﻿function getCounter() {
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
    console.log("check");
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

    let r = serchElem.getElementsByClassName('col-md-2');

    for (let i = 0; i < r.length; i++) {
        let id = (Number)(r[i].getAttribute('post-id'));
        let type = await checkLike(id, 0);
        console.log(type);
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
    let div = document.getElementsByClassName('someComment');
    console.log(div);
    for (let i = 0; i < div.length; i++) {
        let PostId = (Number)(div[i].getAttribute('post-id'));
        let commentId = (Number)(div[i].getAttribute('comment-id'));
        let type = await checkLike(commentId, 1);
        if (type != null) {
            let rating = document.getElementById('comment-rating-' + commentId);
            console.log(rating);
            if (type != 0) {
                console.log(type);
                switch (type) {
                    case 1: rating.nextElementSibling.firstElementChild.setAttribute('fill', 'green'); break;
                    case 2: rating.nextElementSibling.nextElementSibling.firstElementChild.setAttribute('fill', 'red'); break;
                }
            }
        }
    }
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

    if (responce.json != null) {
        let div = this.document.createElement('div');
        div.id = 'post-block-' + number;
        div.innerHTML = await responce.text();
        document.getElementsByClassName('col-md-9')[0].append(div);
        await ColorPostLike(div);
    }
}

async function postArticle(postId) {

    event.preventDefault();
    var Article = new FormData(this.event.target);

    let responce = await fetch('/home/comment', {
        method: "POST",
        body: Article
    })

    document.getElementById('formElem-' + postId).children[1].value = '';

    if (responce.status != 401) {
        let div = document.createElement('div');
        div.className = "border rounded mt-1 p-1";
        div.style = "line-height: 10px";
        let span = document.createElement('span');
        span.className = "text-secondary";
        let fromName = '@Context.User.Identity.Name';
        let ref = document.createElement('a');
        ref.href = '/users/userpage?username=' + fromName;
        ref.append(document.createTextNode(fromName));
        span.append(ref);
        span.append(document.createTextNode(' сейчас'));
        div.append(span);
        let pre = document.createElement('pre');
        pre.className = "mt-3";
        pre.style = "overflow:hidden;";
        pre.innerHTML = Article.get('Text');
        div.append(pre);
        document.getElementById('post-comment-block-' + postId).append(div);
    }
    else
        window.location.href = 'account/login';
}