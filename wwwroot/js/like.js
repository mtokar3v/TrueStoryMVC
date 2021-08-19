async function like(button, type, postId, commentType) {
    let url = '/home/like';
    let responce = await fetch(url, {
        method: 'post',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ PostId: postId, LikeType: type, CommentType: commentType })
    });

    let json = await responce.json();

    if (json != null) {
        let html
        if (commentType == 0)
            html = document.getElementById("post-rating-" + postId);
        else if (commentType == 1)
            html = document.getElementById("comment-rating-" + postId);
        else
            return;

        let count = (Number)(html.textContent);
        count += json.result;
        html.innerHTML = count;
    }
    else
        window.location.href = 'account/login';
}