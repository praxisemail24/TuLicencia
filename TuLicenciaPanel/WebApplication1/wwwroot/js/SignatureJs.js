window.requestAnimFrame = (function (callback) {
    return (
        window.requestAnimationFrame ||
        window.webkitRequestAnimationFrame ||
        window.mozRequestAnimationFrame ||
        window.oRequestAnimationFrame ||
        window.msRequestAnimaitonFrame ||
        function (callback) {
            window.setTimeout(callback, 1000 / 60);
        }
    );
})();

const defaultOptions = {
    selector: '#app',
    cursor: 5,
    color: '#000000',
    width: 600,
    height: 340,
}

const SignatureJs = function(opts = {}) {
    let container = null;
    let options = Object.assign(defaultOptions, opts)
    let canvas = null;
    let ctx = null;

    let drawing = false;
    let mousePos = { x: 0, y: 0 };
    let lastPos = mousePos;

    const initialize = () => {
        container = document.querySelector(options.selector)
        canvas = document.createElement('canvas');
        ctx = canvas.getContext('2d');

        canvas.classList.add('border', 'bg-gray-50', 'rounded-xl');
        canvas.setAttribute('width', options.width);
        canvas.setAttribute('height', options.height);
        
        canvas.addEventListener('mousedown', onMouseDown, false);
        canvas.addEventListener('mouseup', onMouseUp, false);
        canvas.addEventListener('mousemove', onMouseMove, false);
        canvas.addEventListener('touchstart', onTouchStart, false);
        canvas.addEventListener('touchend', onTouchEnd, false);
        canvas.addEventListener('touchleave', onTouchLeave, false);
        canvas.addEventListener('touchmove', onTouchMove, false);

        if(container) {
            container.append(canvas);
        }
        
        drawLoop();
    }

    const onMouseDown = (e) => {
        drawing = true;
        lastPos = getMousePos(canvas, e);
    }

    const onMouseUp = (e) => {
        drawing = false;
    }

    const onMouseMove = (e) => {
        mousePos = getMousePos(canvas, e)
    }

    const onTouchStart = (e) => {
        mousePos = getTouchPos(canvas, e);
        e.preventDefault();
        var touch = e.touches[0];
        var mouseEvent = new MouseEvent("mousedown", {
            clientX: touch.clientX,
            clientY: touch.clientY,
        });
        canvas.dispatchEvent(mouseEvent);
    }

    const onTouchEnd = (e) => {
        e.preventDefault();
        var mouseEvent = new MouseEvent("mouseup", {});
        canvas.dispatchEvent(mouseEvent);
    }

    const onTouchLeave = (e) => {
        e.preventDefault();
        var mouseEvent = new MouseEvent("mouseup", {});
        canvas.dispatchEvent(mouseEvent);
    }

    const onTouchMove = (e) => {
        e.preventDefault();
        var touch = e.touches[0];
        var mouseEvent = new MouseEvent("mousemove", {
            clientX: touch.clientX,
            clientY: touch.clientY,
        });
        canvas.dispatchEvent(mouseEvent);
    }

    const getMousePos = (canvasDom, mouseEvent) => {
        var rect = canvasDom.getBoundingClientRect();
        return {
            x: mouseEvent.clientX - rect.left,
            y: mouseEvent.clientY - rect.top,
        };
    }
    
    const getTouchPos = (canvasDom, touchEvent) => {
        var rect = canvasDom.getBoundingClientRect();
        return {
            x: touchEvent.touches[0].clientX - rect.left,
            y: touchEvent.touches[0].clientY - rect.top,
        };
    }

    const renderCanvas = () => {
        if (drawing) {
            ctx.strokeStyle = options.color;
            ctx.beginPath();
            ctx.moveTo(lastPos.x, lastPos.y);
            ctx.lineTo(mousePos.x, mousePos.y);
            ctx.lineWidth = options.cursor;
            ctx.stroke();
            ctx.closePath();
            lastPos = mousePos;
        }
    }
    
    const clearCanvas = () => {
        canvas.width = canvas.width;
    }
    
    const drawLoop = () => {
        requestAnimFrame(drawLoop);
        renderCanvas();
    }

    initialize();
    drawLoop();

    return {
        container,
        canvas,
        ctx,
        clear: clearCanvas,
        img: () => canvas.toDataURL(),
    }
}