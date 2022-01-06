async function getAvatar(uri) {
    let url = `${uri}/users/GetAvatar`;
    let responce = await fetch(url);

    let json = await responce.json();
    let img = document.getElementById('you-pic');

    if (json.data != null) {
        img.src = "data:image/png;base64," + json.data;
    }
}