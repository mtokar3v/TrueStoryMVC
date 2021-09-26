function getCounter() {
    let counter = 0;
    return function () {
        return counter++;
    }
}
let count = getCounter();

function addBinArray(images) {
    return function (img) {
        images.push(img);
        return images;
    }
}

function delBinArray(images) {
    return function (id) {
        delete images[id];
    }
}

function isBlank(str) {
    return (str.length === 0 || !str.trim());
}

let images = [];
let addImg = addBinArray(images);
let delImg = delBinArray(images);

async function sendData() {
    if (validation()) {
        let text = [];
        let TextArea = document.getElementsByTagName('textarea');

        for (let i = 0; i < TextArea.length; i++) {
            if (!isBlank(TextArea[i].value)) {
                text.push(TextArea[i].value);
            }
            else {
                document.getElementsByClassName('someText')[i].removeAttribute('field-type');
            }
        }

        let scheme = '';
        let rows = document.getElementsByClassName('row');
        for (let i = 0; i < rows.length; i++)
            if (rows[i].hasAttribute('field-type')) {
                scheme += rows[i].getAttribute('field-type') + ' ';
            }

        let header = document.getElementById('header-input').value;
        let tagsline = document.getElementById('tags-input').value;

        await fetch('/home/CreatePost', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ Header: header, Texts: text, Images: images, TagsLine: tagsline, Scheme: scheme })
        });
        window.location.href = '/Home/Hot';
    }
}

function ReadURL(input, i) {
    let file = input.files[i];
    let reader = new FileReader();

    reader.readAsDataURL(file);

    let div = document.createElement("div");
    let img = document.createElement("img");
    reader.onloadend = function () {
        img.src = reader.result;
    }


    div.className = "someImage row";
    div.setAttribute("field-type", "image");
    let number = count();
    div.id = "image-" + number;
    div.setAttribute("number", number);
    let col2 = document.createElement("div");
    col2.className = "col-2";
    let p = document.createElement("p");
    p.innerHTML = 'x';
    p.style.textAlign = "center";

    //сделать более универсальным: поиск по id, а не parent parent
    p.addEventListener("click", (e) => deleteImg(e.target.parentElement.parentElement));
    col2.append(p);
    div.append(col2);
    let col10 = document.createElement("div");
    col10.className = "col-10";
    col10.append(img);
    div.append(col10);

    let form = document.getElementsByTagName('form')[0];
    form.lastElementChild.previousElementSibling.previousElementSibling.after(div);

    img.onload = function () {
        let k = img.height / img.width;
        let width = document.getElementsByClassName("col-10")[0].clientWidth;
        img.width = width;
        img.height = width * k;
    }
}

function ReadBinary(input, i) {
    let file = input.files[i];
    let reader = new FileReader();
    reader.readAsArrayBuffer(file);

    reader.onloadend = function () {
        addImg(Array.from(new Uint8Array(reader.result)));
    }
}

function readFile(input) {
    for (let i = 0; i < input.files.length; i++) {
        ReadURL(input, i);
        ReadBinary(input, i);
    }
}

function deleteImg(divUpperImg) {
    delImg(divUpperImg.getAttribute("number"));
    divUpperImg.remove();
}

function deleteText(divUpperText) {
    divUpperText.remove();
}

function addText() {
    let divText = document.createElement("div");
    divText.className = "someText row";
    divText.setAttribute("field-type", "text");

    let col2 = document.createElement("div");
    col2.className = "col-2";
    let p = document.createElement("p");
    p.innerHTML = 'x';
    p.style.textAlign = "center";
    //сделать более универсальным: поиск по id, а не parent parent
    p.addEventListener("click", (e) => deleteText(e.target.parentElement.parentElement));

    col2.append(p);
    divText.append(col2);

    let col10 = document.createElement("div");
    col10.className = "col-10";

    let div = document.createElement("div");
    div.className = "form-group";

    let textarea = document.createElement("textarea");
    textarea.placeholder = "Введите текст";
    textarea.className = "form-control autotextarea";

    textarea.addEventListener('keyup', function () {
        if (this.scrollTop > 0) {
            this.style.height = this.scrollHeight + "px";
        }
    });

    div.append(textarea);

    col10.append(div);
    divText.append(col10);

    let form = document.getElementsByTagName('form')[0];
    form.lastElementChild.previousElementSibling.previousElementSibling.after(divText);
}