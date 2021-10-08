async function changePic(input, uri) {
    let file = input.files[0];
    let reader = new FileReader();

    reader.readAsArrayBuffer(file);

    reader.onloadend = async function () {
        let url = uri + '/Users/UpdateAvatar';
        await fetch(url, {
            method: 'post',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ Data: Array.from(new Uint8Array(reader.result)) })
        });
        window.location.reload();
    }

}