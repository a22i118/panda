'use strict'

class Hai {
    static Manzu = 0;
    static Pinzu = 1;
    static Souzu = 2;
    static Jihai = 3;

    static Render(ctx, x, y, w, h, type, index) {
        const img = Hai.sImages[0];
        const dx = img.width / 10;
        const dy = img.height / 4;
        ctx.drawImage(img, dx * (index - 1), dy * type, dx, dy, x, y, w, h);
    }

    static sImages = [];

    static LoadImage(callback, images) {
        // すべてのファイルのロードが完了したらcallbackを呼び出す
        const partialCallback = () => {
            numComplete++;
            if (numComplete == numFiles) {
                callback();
            }
        }

        load(Hai.sImages);

        function load(array) {
            for (let val of images) {
                const img = new Image();
                img.addEventListener("load", partialCallback, false);
                img.src = "img/" + val;
                array.push(img);
            }
        }

        // 総ファイル数
        const numFiles = images.length;
        // ロードが完了したファイル数
        let numComplete = 0;
    }
}

export default Hai;
export { Hai };
