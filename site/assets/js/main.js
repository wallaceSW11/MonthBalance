AOS.init({
  duration: 1000,
  once: true,
  offset: 100
});

// Fecha o menu mobile ao clicar em qualquer nav-link
document.addEventListener('DOMContentLoaded', function () {
  const navbarToggler = document.querySelector('.navbar-toggler');
  const navLinks = document.querySelectorAll('#navbarNav .nav-link');
  navLinks.forEach(function (link) {
    link.addEventListener('click', function () {
      if (navbarToggler && window.getComputedStyle(navbarToggler).display !== 'none') {
        navbarToggler.click();
      }
    });
  });
});
