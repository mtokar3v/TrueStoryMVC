
function printError(text) {
    p = document.createElement('p');
    let id = 'error' + Math.random();
    p.id = id;
    p.className = 'error-message';
    p.innerHTML = text;
    document.getElementsByClassName("error-block")[0].append(p);
    setTimeout(() => document.getElementById(id).remove(), 2000);
}

function BlankBlockValidation() {
    let result = false;

    //проверка текстовых полей
    for (let i = 0; i < document.getElementsByClassName('someText').length; i++) {
        result |= !isBlank(document.getElementsByClassName('someText')[i].getElementsByTagName('input')[i].value);
    }

    //проверка изображений
    if (document.getElementsByClassName('someImage').length == 0)
        result |= false;
    else
        result |= true;

    if (result == false)
        printError('Пост должен иметь хотя бы один блок: текст, изображение');

    return result;
}

function validation() {

    let result = true;
    //Проверка заголовка
    result &= BlankStringValidate(document.getElementById('header-input').value, "Введите название заголовка");

    //Проверка тегов
    result &= BlankStringValidate(document.getElementById('tags-input').value, "Введите теги");

    result &= BlankBlockValidation();

    return result;
}

function isBlank(str) {
    return (str.length === 0 || !str.trim());
}

function BlankStringValidate(value, text) {
    let word = value;
    if (isBlank(word)) {
        printError(text);
        return false;
    }
    else {
        return true;
    }
}