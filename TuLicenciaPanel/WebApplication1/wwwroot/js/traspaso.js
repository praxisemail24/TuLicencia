const { createApp } = Vue

axios.defaults.headers.common['Authorization'] = `Bearer ${jwtToken}`;

const app = createApp({
    data() {
        return {
            loading: false,
            loadingSeller: false,
            loadingBuyer: false,
            loadingTransfer: false,
            loadingUpload: false,
            loadingStatus: false,
            loadingCase: false,
            loadingAsigned: false,
            loadingRadicated: false,
            loadingRadication: false,
            model: {
                id: 0,
                seller: {
                    cl_pueblo: {}
                },
                buyer: {
                    cl_pueblo: {}
                },
                documents: [],
                hasContract: false,
                state: 0,
                asignation: {},
            },
            documents: [],
            previewFile: {
                type: 'img',
                src: null,
            },
            transferBoss: false,
            villages: [],
            radicators: [],
            revisedStatusFiles: false,
            loadingBuilderCaseFile: false,
        }
    },
    computed: {
        hasEdit() {
            return this.model.state === 0 || this.model.state === 1;
        },
        radicationStateText() {
            if (this.model.radicationState === 1) {
                return 'Aprobado';
            } else if (this.model.radicationState === 2) {
                return 'Denegado/Subsanable';
            } else if (this.model.radicationState === 3) {
                return 'Denegado/Final';
            } else {
                return 'Pendiente';
            }
        },
        caseFileUrl() {
            return `${baseApiUrlEndPoint}/pdf/consolidado/${this.model.trId}/${this.model.id}`
        },
    },
    watch: {
        'model.hasContract': {
            deep: true,
            handler(f) {
                this.renderFiles();
                if (f) {
                    this.model.paymentDate = null;
                    this.model.paymentAmount = null;
                }
            }
        }
    },
    methods: {
        asyncData() {
            axios.get(`${baseApiUrlEndPoint}/pueblos`).then(({ data }) => {
                if (data.success) {
                    this.villages = data.items;
                }
            });
            axios.get(`${baseApiUrlEndPoint}/Administrador/Radicadores`).then(({ data }) => {
                if (data.success) {
                    this.radicators = data.items;
                }
            });
        },
        loadAsync() {
            this.loading = true;
            axios.get(`${baseApiUrlEndPoint}/traspaso/${params.id}/show`).then(({ data }) => {
                if (data.success) {
                    this.model = {
                        ...data.data,
                        paymentDate: data.data.paymentDate ? data.data.paymentDate.substring(0, 10) : null,
                    };
                    this.transferBoss = this.model.transferType === "owner";
                    this.revisedStatusFiles = this.model.revisedStatus && this.model.evaluationStatus

                    document.title = `Traspaso de Vehículos - TU LICENCIA`
                }
                this.renderFiles();
            }).finally(() => {
                this.loading = false;
            })
        },
        renderFiles() {
            this.documents = [...this.model.documents];
            if (!this.model.hasContract) {
                var index = this.documents.findIndex(x => x.ar_pos === 5);
                if (index === -1 && this.model.id > 0) {
                    this.documents.push({
                        ar_id: 9999,
                        ar_pos: 5,
                        ar_nombre: `${baseApiUrlEndPoint}/Traspaso/gen-billofsale/${this.model.id}/1`,
                    });
                }
            }
            ImagesManager("5", this.$refs.divImagen).render(this.documents);
            //if (!this.model.hasContract) {
            //    $('#card-file-5 .btn-eliminar').css({ display: 'none' });
            //    $('#card-file-5 .btn-editar').css({ display: 'none' });
            //}
            $('.btn-preview', this.$refs.divImagen).on('click', (e) => {
                var el = e.target.tagName == 'I' ? e.target.parentElement : e.target;
                this.previewFile = {
                    type: $(el).attr('data-type'),
                    src: $(el).attr('data-url'),
                }
                $(this.$refs.modalPreview).modal('show');
            });
            $('.btn-editar', this.$refs.divImagen).on('click', (e) => {
                $(e.target).closest('.btn-list').find('input').trigger('click');
            });
            $('.custom-file-input', this.$refs.divImagen).on('change', (e) => {
                if (e.target.files.length > 0) {
                    var el = $(e.target)
                    this.upload(el.attr('data-id'), el.attr('data-pos'), e.target.files[0]);
                }
            });
            $('.btn-eliminar', this.$refs.divImagen).on('click', (e) => {
                e.stopPropagation();
                e.preventDefault();
                var ar_id = $(e.target.tagName === 'I' ? $(e.target).parent() : e.target).attr('id');
                console.log('delete', { ar_id })
                Swal.fire({
                    title: '¿Estás seguro de eliminar este archivo?',
                    text: "No podrás revertir este proceso!",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Sí, eliminar!',
                    cancelButtonText: 'Cancelar'
                }).then((result) => {
                    if (result.isConfirmed) {
                        this.loadingUpload = true;
                        axios.delete(baseApiUrlEndPoint + '/Archivo/' + ar_id).then(({ data }) => {
                            if (data.success) {
                                toastr.success(data.message);

                                this.loadAsync();
                            } else {
                                toastr.error(data.message);
                            }
                        }).finally(() => {
                            this.loadingUpload = false;
                        });
                    }
                });
            });
        },
        upload(id, pos, file) {
            this.loadingUpload = true;
            var form = new FormData();
            form.append('file', file);
            form.append('item', JSON.stringify({
                ar_id: id,
                ar_pos: pos,
                frm_id: this.model.id,
                tr_id: this.model.trId,
                pg_id: this.model.pgId ? this.model.pgId : 0,
                ar_estado: 1,
                ar_fecha: new Date(),
                cl_cliente: { cl_id: this.model.buyer.cl_id },
                tr_tramite: { tr_id: this.model.trId },
            }));
            axios.post(`${baseApiUrlEndPoint}/Archivo/Upload`, form, {
                headers: {
                    ContentType: 'multipart/form-data',
                },
            }).then(({ data }) => {
                if (data.success) {
                    toastr.success(data.message);

                    this.loadAsync();
                } else {
                    toastr.error(data.message);
                }
            }).finally(() => {
                this.loadingUpload = false;
            });
        },
        updateCustomer(customer) {
            return new Promise((resolve, reject) => {
                axios.put(`${baseApiUrlEndPoint}/Cliente`, customer).then(({ data }) => {
                    if (data.success) {
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                    resolve(data);
                }).catch(error => {
                    reject(error);
                });
            });
        },
        updateSeller() {
            this.loadingSeller = true;
            this.updateCustomer(this.model.seller).then((rs) => {
                console.dir(rs);
            }).finally(() => {
                this.loadingSeller = false;
            });
        },
        updateBuyer() {
            this.loadingBuyer = true;
            this.updateCustomer(this.model.buyer).then((rs) => {
                console.dir(rs);
            }).finally(() => {
                this.loadingBuyer = false;
            });
        },
        submitTransfer() {
            if (!this.model.hasContract) {
                if (this.model.paymentAmount === 0 || this.model.paymentAmount === null) {
                    toastr.error('Se requiere monto a pagar para generar el contrato compra y venta.');
                    return;
                }
                if (this.model.paymentDate === null) {
                    toastr.error('Se requiere fecha de pago para generar el contrato compra y venta.');
                    return;
                }
            }

            this.loadingTransfer = true;

            this.model.transferType = this.transferBoss ? "owner" : "";
            axios.put(`${baseApiUrlEndPoint}/traspaso/update`, this.model).then(({ data }) => {
                if (data.success) {
                    toastr.success(data.message);
                } else {
                    toastr.error(data.message);
                }
            }).catch(error => {
                toastr.error(error);
            }).finally(() => {
                this.loadingTransfer = false;
            });
        },
        submitChangeCase() {
            this.loadingCase = true;
            axios.post(`${baseApiUrlEndPoint}/traspaso/change-case`, {
                id: this.model.id,
                state: this.model.state + 1,
            }).then(({ data }) => {
                if (data.success) {
                    toastr.success(data.message);

                    this.loadAsync();
                } else {
                    toastr.error(data.message);
                }
            }).catch(error => {
                toastr.error(error);
            }).finally(() => {
                this.loadingCase = false;
            });
        },
        submitChangeStatus() {
            this.loadingStatus = true;
            axios.put(`${baseApiUrlEndPoint}/traspaso/change-status/${this.model.id}`, {
                revised: this.model.revisedStatus,
                evaluation: this.model.evaluationStatus,
            }).then(({ data }) => {
                if (data.success) {
                    toastr.success(data.message);

                    this.revisedStatusFiles = true;
                } else {
                    toastr.error(data.message);
                }
            }).catch(error => {
                toastr.error(error);
            }).finally(() => {
                this.loadingStatus = false;
            });
        },
        submitAssign() {
            if (!this.model.revisedStatus || !this.model.evaluationStatus) {
                toastr.warning('Revisión pendiente de documentos y multas del cliente.');
                return;
            }

            if (this.model.radicatorId === null) {
                toastr.warning('Seleccione un radicador.');
                return;
            }

            this.loadingAsigned = true;
            axios.put(`${baseApiUrlEndPoint}/traspaso/radicator-assign/${this.model.id}`, {
                adminId: userAuth.id,
                radicatorId: this.model.radicatorId,
            }).then(({ data }) => {
                if (data.success) {
                    toastr.success(data.message);

                    this.loadAsync();
                } else {
                    toastr.error(data.message);
                }
            }).catch(error => {
                toastr.error(error);
            }).finally(() => {
                this.loadingAsigned = false;
            });
        },
        submitDocRadicated() {
            this.loadingRadicated = true;
            axios.post(`${baseApiUrlEndPoint}/traspaso/doc-radicated`, {
                radicated: this.model.radicatedStatus,
                id: this.model.id,
            }).then(({ data }) => {
                if (data.success) {
                    toastr.success(data.message);

                    this.loadAsync();
                } else {
                    toastr.error(data.message);
                }
            }).catch(error => {
                toastr.error(error);
            }).finally(() => {
                this.loadingRadicated = false;
            });
        },
        submitRadication() {
            this.loadingRadication = true;
            axios.post(`${baseApiUrlEndPoint}/traspaso/radication-state`, {
                radicationState: this.model.radicationState,
                radicationObservation: this.model.radicationObservation,
                id: this.model.id,
            }).then(({ data }) => {
                if (data.success) {
                    toastr.success(data.message);
                } else {
                    toastr.error(data.message);
                }
            }).catch(error => {
                toastr.error(error);
            }).finally(() => {
                this.loadingRadication = false;
            });
        },
        submitChangeCase() {
            axios.post(`${baseApiUrlEndPoint}/traspaso/change-case`, {
                state: this.model.state + 1,
                id: this.model.id,
            }).then(({ data }) => {
                if (data.success) {
                    toastr.success(data.message);

                    this.loadAsync();
                } else {
                    toastr.error(data.message);
                }
            }).catch(error => {
                toastr.error(error);
            });
        },
        seeFile(type, url) {
            this.previewFile = {
                type,
                src: url,
            }
            $(this.$refs.modalPreview).modal('show');
        },
        builderCaseFile() {
            this.loadingBuilderCaseFile = true;
            axios.get(`${baseApiUrlEndPoint}/pdf/gen-consolidado/${this.model.trId}/${this.model.id}/1`).finally(() => {
                this.loadingBuilderCaseFile = false;
            })
        }
    },
    mounted() {
        this.asyncData();
        this.loadAsync();
    }
});

app.mount('#viewTraspaso');