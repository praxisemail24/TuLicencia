const StripePay = (token = null) => {
    token = token ? token : (window['jwtToken'] !== undefined ? jwtToken : null);
    let stripeCore;

    if (window['Stripe'] !== undefined) {
        stripeCore = Stripe('#PUBLIC_KEY#');
    }

    const init = async(selectorId, data) => {
        const fetchClientSecret = async () => {
            const response = await fetch('#BASE_URL#/create-session', {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(data),
            });

            const { success, message, clientSecret } = await response.json();

            if (!success) {
                var container = document.querySelector(selectorId);
                if (container) {
                    var frames = container.getElementsByTagName('iframe');

                    if (frames.length > 0) {
                        frames[0].style.display = 'none';
                    }

                    var pError = document.createElement('p');
                    pError.innerHTML = message;
                    pError.classList.add('text-red-500', 'p-5', 'text-center', 'm-auto');
                    container.appendChild(pError);
                }
            }

            return clientSecret;
        };

        const checkout = await stripeCore.initEmbeddedCheckout({
            fetchClientSecret,
        });

        checkout.mount(selectorId);

        return checkout;
    }

    const createFrame = (selectorOrEl, baseUrl, params = {}) => {
        if (typeof params.payloadUrl !== 'string') {
            params.payloadUrl = '#BASE_URL#/checkout/success';
        }

        if (Array.isArray(params.installments)) {
            var installments = params.installments.map((c) => {
                return `${c.index}|${c.date.toISOString().substr(0, 10)}|${parseFloat(c.amount).toFixed(6)}`;
            }).concat();

            params.installments = btoa(installments);
        }

        const queryString = new URLSearchParams(params).toString();

        if (typeof selectorOrEl === 'string') {
            selectorOrEl = document.querySelector(selectorOrEl);
        }

        var iframe = document.createElement('iframe');
        iframe.src = `${baseUrl}?${queryString}`;
        iframe.style.width = '100%';
        iframe.style.minHeight = '745px';
        selectorOrEl.replaceChildren();
        selectorOrEl.append(iframe);
    }

    const embed = (selectorOrEl, params = {}) => {
        createFrame(selectorOrEl, `#BASE_URL#/checkout`, params);
    }

    const procedurePayment = (selectorOrEl, trId, clId, payloadUrl = null) => {
        createFrame(selectorOrEl, `#BASE_URL#/checkout/payments/${trId}`, {
            clId,
            payloadUrl,
        });
    }

    const result = () => {
        return new Promise(async(resolve, reject) => {
            try {
                const queryString = window.location.search;
                const urlParams = new URLSearchParams(queryString);
                const sessionId = urlParams.get('session_id');
                const response = await fetch(`#BASE_URL#/session-status/${sessionId}`, {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${token}`
                    },
                });
                const session = await response.json();

                resolve(session);
            } catch (e) {
                reject(e);
            }
        })
    }

    return {
        init,
        embed,
        procedurePayment,
        result,
        getCore: () => stripeCore,
    }
};

const stripeTest = () => {
    StripePay().procedurePayment('#formStripe', 5);
    $('#modalPayment').modal('show');
}