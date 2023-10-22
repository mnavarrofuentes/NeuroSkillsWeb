// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function GetAlertHtml(type, text, strongText = '', error = '', closable = true) {
    return `
<div class="alert alert-${type} alert-dismissible fade show" role="alert">
  ${(strongText ? `<strong>${strongText}</strong>` : ``)}${text}.${(error ? ` Error: ${error}` : ``)}
  ${(closable ? `<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>` : ``)}
</div>
    `;
}