let buttonElement = document.querySelector('#favButton');
let id = document.querySelector('#book-id')
    .textContent;

buttonElement.addEventListener('click', (e) => {
    let targetElement = e.target;
    ToggleClass(targetElement);
});

function ToggleClass(targetElement) {
    if (targetElement.classList.contains('btn-warning')) {
        targetElement.classList.remove('btn-warning');
        targetElement.classList.add('btn-danger');
        targetElement.textContent = 'Unfav';
    } else {
        targetElement.classList.remove('btn-danger');
        targetElement.classList.add('btn-warning');
        targetElement.textContent = 'Fav';
    }
}