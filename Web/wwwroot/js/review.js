let ratingDivElement = document.querySelector('#rating-div');
let ratingStarElements = ratingDivElement.querySelectorAll('.bi');
let ratingInputElement = document.querySelector('input[name="Value"]');

ratingStarElements.forEach(s => s.addEventListener('click', () => {
    ratingStarElements.forEach(rs => {
        rs.classList.remove('bi-star-fill');
        rs.classList.add('bi-star');
    });

    let value = s.dataset.value;

    for (var i = 0; i < value; i++) {
        //bi-star-fill
        let currentElement = ratingStarElements[i];
        ToggleClass(currentElement);
    }

    ratingInputElement.value = value;
}));

function ToggleClass(element) {
    if (element.classList.contains('bi-star')) {
        element.classList.remove('bi-star');
        element.classList.add('bi-star-fill');
    } else {
        element.classList.remove('bi-star-fill');
        element.classList.add('bi-star');
    }
}