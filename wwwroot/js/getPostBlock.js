
function getCounter() {
    let counter = 0;
    return function () {
        return counter++;
    }
}
let count = getCounter();

async function getPostBlockAfterScroll(type) {
    let window_bottom = document.documentElement.getBoundingClientRect().bottom;
    if (Math.round(window_bottom) <= document.documentElement.clientHeight) {
        await getPostBlock(type);
    }
}

async function checkLike(id) {
    let responce = await fetch('/home/checkLike', {
        method: 'post',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ PostId: id })
    });
    let json = await responce.json();
    return json == null ? null : json.result;
}

async function getPostBlock(type) {
    let number = count();
    let responce = await fetch('/home/GetPostBlock', {
        method: 'post',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ PostBlockType: type, Number: number })
    });

    let div = this.document.createElement('div');
    div.id = 'post-block-' + number;
    div.innerHTML = await responce.text();
    this.document.getElementsByClassName('col-md-9')[0].append(div);

    let r = div.getElementsByClassName('col-sm-2');
    for (let i = 0; i < r.length; i++) {
        let id = (Number)(r[i].getAttribute('post-id'));
        let type = await checkLike(id);
        if (type != null) {
            let rating = document.getElementById('post-rating-' + id);
            if (type != 0) {
                switch (type) {
                    case 1: rating.previousElementSibling.firstElementChild.setAttribute('fill', 'green'); break;
                    case 2: rating.nextElementSibling.firstElementChild.setAttribute('fill', 'red'); break;
                }
            }
        }
    }
}