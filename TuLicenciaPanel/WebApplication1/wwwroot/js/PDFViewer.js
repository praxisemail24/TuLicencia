var pdfDoc = null,
    pageNum = 1,
    pageRendering = false,
    pageNumPending = null,
    scale = 1.5,
    canvas = document.getElementById('pdf-viewer'),
    ctx = canvas.getContext('2d');

var loader = document.getElementById('loader');
var offsetX, offsetY;

const toBase64 = (file) => new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = reject;
});

/**
* Cargar el PDF.
*/
const loadAsync = (uri) => {
    loader.style.display = 'flex';
    canvas.style.display = 'none';
    pdfjsLib.getDocument(uri).promise.then(function (pdfDoc_) {
        pdfDoc = pdfDoc_;
        document.getElementById('pageCount').textContent = pdfDoc.numPages;

        document.getElementById('downloadDoc').setAttribute('href', uri);
        // Renderizar la primera página
        renderPage(pageNum);
    }).finally(() => {
        loader.style.display = 'none';
        canvas.style.display = 'block';
    });
}

loadAsync(url);

/**
 * Renderizar la página del PDF.
 * @param num Página a renderizar.
 */
function renderPage(num) {
    pageRendering = true;
    // Usar el getPage() método de pdf.js para obtener la página
    pdfDoc.getPage(num).then(function (page) {
        var viewport = page.getViewport({ scale: scale });

        canvas.height = viewport.height;
        canvas.width = viewport.width;
        loader.height = viewport.height;

        var renderContext = {
            canvasContext: ctx,
            viewport: viewport
        };
        var renderTask = page.render(renderContext);

        // Esperar a que la página se renderice
        renderTask.promise.then(function () {
            pageRendering = false;
            if (pageNumPending !== null) {
                renderPage(pageNumPending);
                pageNumPending = null;
            }
        });
    });

    // Actualizar el número de página
    document.getElementById('pageNum').textContent = num;
}

/**
 * Verificar si hay una página pendiente para renderizar.
 * @param num Página a renderizar.
 */
function queueRenderPage(num) {
    if (pageRendering) {
        pageNumPending = num;
    } else {
        renderPage(num);
    }
}

/**
 * Mostrar la página anterior.
 */
function onPrevPage() {
    if (pageNum <= 1) {
        return;
    }
    pageNum--;
    queueRenderPage(pageNum);
}

document.getElementById('prevPage').addEventListener('click', onPrevPage);

/**
 * Mostrar la siguiente página.
 */
function onNextPage() {
    if (pageNum >= pdfDoc.numPages) {
        return;
    }
    pageNum++;
    queueRenderPage(pageNum);
}

document.getElementById('nextPage').addEventListener('click', onNextPage);

/**
 * Aumentar el zoom.
 */
function onZoomIn() {
    scale += 0.1;
    renderPage(pageNum);
}

document.getElementById('zoomIn').addEventListener('click', onZoomIn);

/**
 * Disminuir el zoom.
 */
function onZoomOut() {
    if (scale <= 0.5) {
        return;
    }
    scale -= 0.1;
    renderPage(pageNum);
}

document.getElementById('zoomOut').addEventListener('click', onZoomOut);

// Función de cambio de página al hacer scroll
document.getElementById('pdfContainer').addEventListener('scroll', function () {
    const container = document.getElementById('pdfContainer');
    const offSet = 0.8;
    if (container.scrollTop + container.clientHeight >= (container.scrollHeight - offSet)) {
        if (pageNum < pdfDoc.numPages) {
            pageNum++;
            queueRenderPage(pageNum);
            container.scrollTop = 1; // Reset scroll position
        }
    } else if (container.scrollTop === 0 && pageNum > 1) {
        pageNum--;
        queueRenderPage(pageNum);
        container.scrollTop = 1; // Set scroll position to bottom
    }
});

/*document.getElementById('printPdf').addEventListener('click', function () {
    const iframe = document.createElement('iframe');
    iframe.style.position = 'absolute';
    iframe.style.width = '0px';
    iframe.style.height = '0px';
    iframe.src = url;
    document.body.appendChild(iframe);
    iframe.contentWindow.focus();
    iframe.contentWindow.print();
});*/

const pluginSignature = () => {
    let signature = SignatureJs({ selector: '#box-signed' })
    let modalSignature = document.getElementById('modalSignature');
    let imgSignature = document.getElementById('imgSignature');
    let drawSignature = document.getElementById('drawSignature');
    let saveSignature = document.getElementById('saveSignature');

    saveSignature.classList.add('!hidden');

    const moveSignature = (e) => {
        // Obtén el contenedor y sus posiciones de desplazamiento
        const container = document.getElementById('pdfContainer');
        const containerRect = container.getBoundingClientRect();

        // Calcula la nueva posición de la firma dentro del contenedor de PDF
        const newX = e.clientX - containerRect.left - offsetX + container.scrollLeft;
        const newY = e.clientY - containerRect.top - offsetY + container.scrollTop;

        imgSignature.style.left = `${newX}px`;
        imgSignature.style.top = `${newY}px`;
    }

    const saveSignaturePosition = async() => {
        const rect = imgSignature.getBoundingClientRect();
        const canvasRect = canvas.getBoundingClientRect();

        // Calcular posición de la firma relativa al PDF
        const x = rect.left - canvasRect.left + document.getElementById('pdfContainer').scrollLeft;
        const y = canvasRect.height - (rect.top - canvasRect.top + document.getElementById('pdfContainer').scrollTop) - rect.height;

        const rawResponse = await fetch(`${baseApiUrlEndPoint}${queryParams.signUrl}`, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                ...queryParams,
                url,
                x,
                y,
                page: pageNum,
                sign: imgSignature.getAttribute('src'),
                width: rect.width,
                height: rect.height,
            })
        });
        const content = await rawResponse.json();
        
        console.log(content);
    }

    const modalButtonEvent = (btnClass, event, callback) => {
        const btn = modalSignature.getElementsByClassName(btnClass)

        if(btn.length > 0) {
            for (let index = 0; index < btn.length; index++) {
                const element = btn.item(index);
                if(element) {
                    element.addEventListener(event, callback, false);
                }
            }
        }
    }

    const toggleModal = () => {
        if(modalSignature.classList.contains('hidden')) {
            modalSignature.classList.remove('hidden')
        } else {
            modalSignature.classList.add('hidden')
        }
    }

    imgSignature.classList.add('hidden');

    imgSignature.addEventListener('mousedown', (e) => {
        offsetX = e.offsetX;
        offsetY = e.offsetY;
        document.addEventListener('mousemove', moveSignature);
    });

    document.addEventListener('mouseup', () => document.removeEventListener('mousemove', moveSignature));

    drawSignature.addEventListener('click', () => toggleModal());

    modalButtonEvent('cancel', 'click', () => toggleModal());
    modalButtonEvent('clear', 'click', () => signature.clear());
    modalButtonEvent('success', 'click', () => {
        saveSignature.classList.remove('!hidden');
        imgSignature.classList.remove('hidden');
        imgSignature.setAttribute('src', signature.img());
        toggleModal();
    });

    saveSignature.addEventListener('click', () => {
        saveSignaturePosition()
    })
}

pluginSignature()