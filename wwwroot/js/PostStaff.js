async function like(button, type, contentId, fromType, uri) {
    //types:
    //FROM_POST = 0;
    //FROM_COMMENT = 1;
    let url = `${uri}/Content/like`;
    let responce = await fetch(url, {
        method: 'post',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ ContentId: contentId, LikeType: type, FromType: fromType })
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

            html = document.getElementById("post-rating-" + contentId);
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

            html = document.getElementById("comment-rating-" + contentId);
        }
        else
            return;

        let count = (Number)(html.textContent);
        count += json.result;
        html.innerHTML = count;
    }
    else
        window.location.href = uri + '/account/login';
}

async function checkLike(id, fromType, uri) {
    let url = `${uri}/Content/CheckLike?ContentId=${id}&FromType=${fromType}`;
    let responce = await fetch(url);
    return responce.status == 200 ? (await responce.json()).result : null;
}

async function ColorPostLike(serchElem, uri) {

    let r = serchElem.getElementsByClassName('col-2');

    for (let i = 0; i < r.length; i++) {
        let id = (Number)(r[i].getAttribute('post-id'));
        let type = await checkLike(id, 0, uri);

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

//TO DO: make a comments branch support
async function ColorCommentLike(uri) {
    const LIKE = 1;
    const DISLIKE = 2;
    let div = document.getElementsByClassName('someComment');
    for (let i = 0; i < div.length; i++) {
        let commentId = (Number)(div[i].getAttribute('comment-id'));
        let type = await checkLike(commentId, 1, uri);
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

async function CreateBlock(num, html, uri) {
    let div = this.document.createElement('div');
    div.id = 'post-block-' + num;
    div.className = 'postBlock';
    div.innerHTML = html;
    document.getElementsByClassName('col-9')[0].append(div);
    await ColorPostLike(div, uri);
}

async function getPostBlock(type, arg, uri) {
    let num = document.getElementsByClassName('postBlock').length;
    let url = `${uri}/Content/GetPosts?PostBlockType=${type}&Number=${num}&Argument=${arg}`;

    let responce = await fetch(url);

    if (responce.json != null)
        await CreateBlock(num, await responce.text(), uri);
}

async function getPostBlockAfterScroll(type, arg, uri) {
    let window_bottom = document.documentElement.getBoundingClientRect().bottom;
    if (Math.round(window_bottom) <= document.documentElement.clientHeight) {
        await getPostBlock(type, arg, uri);
    }
}

function isBlank(str) {
    return (str.length === 0 || !str.trim());
}

async function DeletePost(id, uri) {
    let url = `${uri}/Content/DeletePost`;
    let responce = await fetch(url, {
        method: 'delete',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ Id: id })
    });

    if (responce.status == 200) {
        window.location.reload();
    }
}
