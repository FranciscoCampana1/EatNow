var divs = document.querySelectorAll('.restaurante');
var primerElemento = divs[0];
var ultimoElemento = divs[divs.length - 1];

var defHeight = 40;
var defWidth = 50;

divs.forEach(function (elemento) {
    elemento.addEventListener('mouseover', function () { Focus(elemento); });
    elemento.addEventListener('mouseenter', function () { elemento.classList.add('selected'); });
    elemento.addEventListener('mouseleave', function () { elemento.classList.remove('selected'); });
});

function Focus(divWatch) {
    divWatch.scrollIntoView({ behavior: "instant", block: "center", inline: "center" });
    UpdateSizeRestaurants();
}

document.addEventListener("DOMContentLoaded", UpdateSizeRestaurants);
window.addEventListener('scroll', UpdateSizeRestaurants);

//pee = setInterval(UpdateSizeRestaurants, 10);

function UpdateSizeRestaurants() {
    var scrollPos = window.scrollY;
    var centroVentana = window.innerHeight / 2;

    divs.forEach(function (elemento) {
        if (elemento.classList.contains('selected')) {
            elemento.style.opacity = 1;
            //elemento.innerHTML = 'Selected';
        }
        else {
            var bounding = elemento.getBoundingClientRect();
            var center = (bounding.top + bounding.bottom) / 2;
            var distanciaCentro = Math.abs(center - centroVentana);
            var escala = distanciaCentro / centroVentana;

            if (escala > 1) escala = 1;
            if (escala < 0.4 && scrollPos != 0) escala = 0;
            //elemento.style.transform = 'scale(' + escala + ')';

            var heightElement = defHeight - (escala * 20);
            var widthElement = defWidth - (escala * 10);

            elemento.style.height = heightElement.toFixed(1) + 'vh';
            elemento.style.width = widthElement.toFixed(1) + '%';
            elemento.style.opacity = (1 - escala) + 0.2;
            //elemento.innerHTML = escala.toFixed(1);
        }
    });

    if (scrollPos == 0) {
        primerElemento.style.height = defHeight + 'vh';
        primerElemento.style.width = defWidth + '%';
        primerElemento.style.opacity = 1;
    }
}