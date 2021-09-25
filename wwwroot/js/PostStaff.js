async function getPostBlockAfterScroll(type, arg) {
    let window_bottom = document.documentElement.getBoundingClientRect().bottom;
    if (Math.round(window_bottom) <= document.documentElement.clientHeight) {
        await getPostBlock(type, arg);
    }
}

async function like(button, type, postId, fromType) {
    let url = '/home/like';
    let responce = await fetch(url, {
        method: 'post',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ PostId: postId, LikeType: type, FromType: fromType })
    });

    let json = await responce.json();

    if (json != null) {
        let html
        if (fromType == 0) {

            if (type == 1) {
                button.setAttribute('fill', 'green');
                button.parentNode.nextElementSibling.nextElementSibling.firstElementChild.setAttribute('fill', 'currentColor');
            }

            if (type == 2) {
                button.setAttribute('fill', 'red');
                button.parentNode.previousElementSibling.previousElementSibling.firstElementChild.setAttribute('fill', 'currentColor');
            }


            html = document.getElementById("post-rating-" + postId);
        }
        else if (fromType == 1) {

            if (type == 1) {
                button.setAttribute('fill', 'green');
                button.parentNode.nextElementSibling.firstElementChild.setAttribute('fill', 'currentColor');
            }

            if (type == 2) {
                button.setAttribute('fill', 'red');
                button.parentNode.previousElementSibling.firstElementChild.setAttribute('fill', 'currentColor');
            }

            html = document.getElementById("comment-rating-" + postId);
        }
        else
            return;

        let count = (Number)(html.textContent);
        count += json.result;
        html.innerHTML = count;
    }
    else
        window.location.href = 'account/login';
}

async function checkLike(id, fromType) {
    let responce = await fetch('/home/checkLike', {
        method: 'post',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ PostId: id, LikeType: 0, FromType: fromType })
    });
    let json = await responce.json();
    return json == null ? null : json.result;
}

async function ColorPostLike(serchElem) {

    let r = serchElem.getElementsByClassName('col-2');

    for (let i = 0; i < r.length; i++) {
        let id = (Number)(r[i].getAttribute('post-id'));
        let type = await checkLike(id, 0);

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

//нужно будет добавить совместимость с ветками комментариев
async function ColorCommentLike() {
    const LIKE = 1;
    const DISLIKE = 2;
    let div = document.getElementsByClassName('someComment');
    console.log(div);
    for (let i = 0; i < div.length; i++) {
        let commentId = (Number)(div[i].getAttribute('comment-id'));
        let type = await checkLike(commentId, 1);
        if (type != null) {
            let rating = document.getElementById('comment-rating-' + commentId);
            if (type != 0) {
                switch (type) {
                    case LIKE: rating.nextElementSibling.firstElementChild.setAttribute('fill', 'green'); break;
                    case DISLIKE: rating.nextElementSibling.nextElementSibling.firstElementChild.setAttribute('fill', 'red'); break;
                }
            }
        }
    }
}

async function CreateBlock(num, html) {
    let div = this.document.createElement('div');
    div.id = 'post-block-' + num;
    div.className = 'postBlock';
    div.innerHTML = html;
    document.getElementsByClassName('col-9')[0].append(div);
    await ColorPostLike(div);
}

async function getPostBlock(type, arg) {
    let num = document.getElementsByClassName('postBlock').length;

    let responce = await fetch('/home/GetPostBlock', {
        method: 'post',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ PostBlockType: type, Number: num, Argument: arg })
    });

    if (responce.json != null)
        await CreateBlock(num, await responce.text());
}

function isBlank(str) {
    return (str.length === 0 || !str.trim());
}

