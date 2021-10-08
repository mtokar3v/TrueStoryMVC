async function getAvatar(uri) {
    let url = uri + '/users/getAvatar';
    let responce = await fetch(url, {
        method: 'post',
        headers: {
            'Accept': 'application/json'
        }
    });

    let json = await responce.json();
    let img = document.getElementById('you-pic');

    if (json.data != null) {
        img.src = "data:image/png;base64," + json.data;
    }
}