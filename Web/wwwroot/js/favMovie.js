let buttonElement = document.querySelector('#favButton');
//let id = document.querySelector('#book-id')
//    .textContent;

buttonElement.addEventListener('click', (e) => {
    let targetElement = e.target;
    ToggleClass(targetElement);
});

function ToggleClass(targetElement) {
    if (targetElement.textContent=='Favorite') {
        targetElement.textContent = 'Unfavorite';
    } else {
        targetElement.textContent = 'Favorite';
    }
}