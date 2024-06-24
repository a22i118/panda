'use strict'

import { Hai } from "./hai.js";

function loadHai() {
    Hai.LoadImage(main,
        [
            'hai.png'
        ]);
}

function main() {
    let before;
    function render(timestamp) {
        timestamp *= 0.001;
        const delta = before === undefined ? 0 : timestamp - before;
        before = timestamp;

        ctx.fillStyle = 'darkgreen';
        ctx.fillRect(0, 0, canvas.width, canvas.height)

        ctx.font = bold(32);
        ctx.textAlign = 'center';
        ctx.textBaseline = 'middle';

        ctx.fillStyle = 'white';
        ctx.font = bold(64);
        ctx.fillText("麻雀", canvas.width / 2, 64);

        const w = 38;
        const h = 54;
        {
            let x = 128 - w;
            const y = 128;
            const array = [
                [Hai.Jihai, 1], [Hai.Jihai, 1], [Hai.Jihai, 1],
                [Hai.Jihai, 2], [Hai.Jihai, 2], [Hai.Jihai, 2],
                [Hai.Jihai, 3], [Hai.Jihai, 3], [Hai.Jihai, 3],
                [Hai.Jihai, 4], [Hai.Jihai, 4], [Hai.Jihai, 4],
                [Hai.Jihai, 7], [Hai.Jihai, 7]
            ];
            for (let val of array) {
                Hai.Render(ctx, x += w, y, w, h, val[0], val[1]);
            }
        }
        {
            let x = 128 - w;
            const y = 192;
            const array = [
                [Hai.Manzu, 1], [Hai.Manzu, 1], [Hai.Manzu, 1],
                [Hai.Manzu, 1], [Hai.Manzu, 2], [Hai.Manzu, 3],
                [Hai.Manzu, 4], [Hai.Manzu, 5], [Hai.Manzu, 6],
                [Hai.Manzu, 7], [Hai.Manzu, 8], [Hai.Manzu, 9],
                [Hai.Manzu, 9], [Hai.Manzu, 9]
            ];
            for (let val of array) {
                Hai.Render(ctx, x += w, y, w, h, val[0], val[1]);
            }
        }
        {
            let x = 128 - w;
            const y = 256;
            const array = [
                [Hai.Souzu, 2], [Hai.Souzu, 2], [Hai.Souzu, 3], [Hai.Souzu, 3], [Hai.Souzu, 4], [Hai.Souzu, 4],
                [Hai.Souzu, 6], [Hai.Souzu, 6],
                [Hai.Souzu, 8], [Hai.Souzu, 8], [Hai.Souzu, 8],
                [Hai.Jihai, 6], [Hai.Jihai, 6], [Hai.Jihai, 6]
            ];
            for (let val of array) {
                Hai.Render(ctx, x += w, y, w, h, val[0], val[1]);
            }
        }
        {
            let x = 128 - w;
            const y = 320;
            const array = [
                [Hai.Manzu, 3], [Hai.Manzu, 3], [Hai.Manzu, 3],
                [Hai.Pinzu, 3], [Hai.Pinzu, 3], [Hai.Pinzu, 3],
                [Hai.Souzu, 3], [Hai.Souzu, 3], [Hai.Souzu, 3],
                [Hai.Jihai, 5], [Hai.Jihai, 5], [Hai.Jihai, 5],
                [Hai.Jihai, 7], [Hai.Jihai, 7]
            ];
            for (let val of array) {
                Hai.Render(ctx, x += w, y, w, h, val[0], val[1]);
            }
        }

        requestAnimationFrame(render);
    }
    requestAnimationFrame(render);
}
window.onload = loadHai;

canvas.addEventListener('pointerdown', e => {
    const rect = canvas.getBoundingClientRect();
    const x = (e.clientX - rect.left) / (rect.width / canvas.width);
    const y = (e.clientY - rect.top) / (rect.height / canvas.height);
});

addEventListener('pointermove', e => {
    const rect = canvas.getBoundingClientRect();
    const x = (e.clientX - rect.left) / (rect.width / canvas.width);
    const y = (e.clientY - rect.top) / (rect.height / canvas.height);
});

canvas.addEventListener('pointerup', e => {
    const rect = canvas.getBoundingClientRect();
    const x = (e.clientX - rect.left) / (rect.width / canvas.width);
    const y = (e.clientY - rect.top) / (rect.height / canvas.height);
});
