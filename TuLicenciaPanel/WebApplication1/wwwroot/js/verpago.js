async function traeDatoPago(codigoPago) {
   try {
       const response = await fetch(baseApiUrlEndPoint+`/Multa/traeDatoPago/${codigoPago}`, {
            headers: {
               'Authorization': `Bearer ${jwtToken}`
            }
        });
        if (!response.ok) {
            throw new Error('Error en la solicitud');
        }
        const data = await response.json();

        document.getElementById('nombre').innerText = data.nombre;
        document.getElementById('codigoPago').innerText = data.codigoPago;
       document.getElementById('transaccion').innerText = data.transaccion;
       document.getElementById('telefono').innerText = data.telefono;
       document.getElementById('direccion').innerText = data.direccion;
       document.getElementById('fecha').innerText = data.fecha;
       document.getElementById('fechatabla').innerText = data.fecha;
        document.getElementById('tramite').innerText = data.tramite;
       document.getElementById('cuota').innerText =  data.subtotal;
       document.getElementById('monto').innerText = '$' + data.total;
       document.getElementById('subtotal').innerText = '$' +data.monto;
       document.getElementById('total').innerText = '$' +data.monto;
    } catch (error) {
        console.error('Error al obtener los datos del pago:', error);
    }
}

function getQueryParameter(name) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(name);
}

document.addEventListener('DOMContentLoaded', () => {
    const codigoPago = getQueryParameter('codigopago');
    if (codigoPago) {
        traeDatoPago(codigoPago);
    } else {
        console.error('Código de pago no especificado en la URL');
    }
});

function exportPDF() {
    const element = document.getElementById('card3');
    const options = {
        margin: 1,
        filename: 'detalle_pago.pdf',
        image: { type: 'jpeg', quality: 0.98 },
        html2canvas: { scale: 2 },
        jsPDF: { unit: 'in', format: 'letter', orientation: 'portrait' }
    };

    html2pdf().from(element).set(options).save();
}