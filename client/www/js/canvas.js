'use strict'

// JavaScriptでブラウザバックを “ほぼ完全禁止” する方法
// https://qiita.com/donkey-maru/items/c5cd17007f8a7ca8fb18
history.pushState(null, null, location.href);
window.addEventListener('popstate', (e) => {
    history.go(1);
});

addEventListener('DOMContentLoaded', function () {
    var canvas = document.getElementById('canvas');
    var styles = canvas.getAttribute('style') || '';
    var context = canvas.getContext('2d');
    // var input = document.getElementById('input');

    // canvasが見えるように、色を付けます
    // context.fillStyle = 'rgba(0,0,16, 0.66)';
    // context.fillRect(0, 0, canvas.width, canvas.height);

    var onResize = canvas => {
        var scale = Math.min(window.innerWidth / canvas.width, window.innerHeight / canvas.height);
        var transform = 'scale(' + scale + ',' + scale + ');';

        canvas.setAttribute('style', styles +
            '    -moz-transform: ' + transform +
            '     -ms-transform: ' + transform +
            '      -o-transform: ' + transform +
            '         transform: ' + transform +
            ' -webkit-transform-origin: center center;' +
            '    -moz-transform-origin: center center;' +
            '     -ms-transform-origin: center center;' +
            '      -o-transform-origin: center center;' +
            '         transform-origin: center center;'
        );

        // input.setAttribute('style', "position: absolute; left: "
        //     + (this.window.innerWidth / 2 - input.offsetWidth / 2) +
        //     // + 16 +
        //     "px; margin-left: "
        //     + 0 +
        //     "px; top: "
        //     // + (this.window.innerHeight - input.offsetHeight - 1) +
        //     + 16 +
        //     "px; margin-top: "
        //     + 0 +
        //     "px");
    }

    onResize(canvas);
    window.addEventListener('resize', () => onResize(canvas), false);
});

const font_small = '32px sans-serif';
const font_large = '96px sans-serif';
const font_default = '"M PLUS Rounded 1c"';

const midium = (size) => { return `${size}px ${font_default}`; }
const bold = (size) => { return `bold ${size}px ${font_default}`; }

const canvas = document.getElementById('canvas');
const ctx = canvas.getContext('2d');

ctx.font = bold(32);
ctx.textAlign = 'center';
ctx.textBaseline = 'middle';

const saturate = (x) => { return Math.max(0, Math.min(1, x)); }

const smoothstep = (edge0, edge1, x) => {
    x = saturate((x - edge0) / (edge1 - edge0));
    return x * x * (3 - 2 * x);
}

const lerp = (a, b, rate) => { return a + (b - a) * rate; }
