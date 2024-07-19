document.addEventListener("DOMContentLoaded", () => {
    const $file = document.getElementById("file-upload");
    const $nameImage = document.getElementById("name-image");
    const $priview = document.getElementById("preview");

    $file.addEventListener("change", (e) => {
        const file = e.target.files[0];
        console.log({ file });
        $nameImage.textContent = file.name;

        const reader = new FileReader();

        reader.onload = (e) => {
            console.log(e.target.result);

            $priview.src = e.target.result;
            $priview.style.display = "block";
        };

        reader.readAsDataURL(file);
    });
})