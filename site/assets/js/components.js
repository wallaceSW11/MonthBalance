/**
 * Injeta a navbar no elemento com id="navbar-placeholder"
 * @param {string} activePage - nome da página ativa: 'sobre', 'produtos', 'contato'
 * @param {string} basePath - caminho relativo para a raiz do site (ex: '../' para pages/)
 */
function renderNavbar(activePage, basePath = '') {
  const links = [
    { label: 'Sobre',    href: `${basePath}pages/sobre.html`,            key: 'sobre' },
    { label: 'Produtos', href: `${basePath}index.html#produtos`,          key: 'produtos' },
    { label: 'Contato',  href: `${basePath}index.html#contato`,           key: 'contato' },
  ];

  const navItems = links.map(link => `
    <li class="nav-item">
      <a class="nav-link${activePage === link.key ? ' active' : ''}" href="${link.href}">${link.label}</a>
    </li>`).join('');

  const html = `
    <nav class="navbar navbar-expand-lg navbar-dark fixed-top p-3" id="mainNav">
      <div class="container">
        <a class="navbar-brand fw-bold" href="${basePath}index.html">
          <i class="fa-solid fa-layer-group text-primary me-2"></i>WallTech
        </a>
        <button
          class="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
          aria-controls="navbarNav"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav ms-auto">
            ${navItems}
          </ul>
        </div>
      </div>
    </nav>`;

  const placeholder = document.getElementById('navbar-placeholder');
  if (placeholder) placeholder.outerHTML = html;
}

/**
 * Injeta o modal de imagem ampliada no body e ativa o clique em imagens com classe .playable-image
 */
function renderImageModal() {
  const html = `
    <div class="modal fade" id="imageModal" tabindex="-1" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered modal-fullscreen">
        <div class="modal-content bg-black border-0">
          <div class="modal-body d-flex align-items-center justify-content-center position-relative p-2">
            <button type="button" class="btn-close btn-close-white position-absolute top-0 end-0 m-3" data-bs-dismiss="modal" aria-label="Fechar"></button>
            <img src="" id="modalImage" alt="Imagem ampliada" style="max-width: 100%; max-height: 95vh; object-fit: contain; border-radius: 8px;">
          </div>
        </div>
      </div>
    </div>`;

  document.body.insertAdjacentHTML('beforeend', html);

  document.querySelectorAll('.playable-image').forEach(function (img) {
    img.style.cursor = 'pointer';
    img.addEventListener('click', function () {
      document.getElementById('modalImage').setAttribute('src', this.getAttribute('src'));
      new bootstrap.Modal(document.getElementById('imageModal')).show();
    });
  });
}

/**
 * Injeta o footer no elemento com id="footer-placeholder"
 */
function renderFooter() {
  const year = new Date().getFullYear();
  const html = `
    <footer class="py-4 bg-black border-top border-secondary">
      <div class="container text-center">
        <p class="m-0 text-white-50 small">
          &copy; ${year} WallTech — Tecnologia que simplifica.
        </p>
      </div>
    </footer>`;

  const placeholder = document.getElementById('footer-placeholder');
  if (placeholder) placeholder.outerHTML = html;
}
