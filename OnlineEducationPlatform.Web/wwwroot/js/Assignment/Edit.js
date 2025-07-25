// Remove required/data-val-required from AssignmentFile input on page load
document.addEventListener('DOMContentLoaded', function () {
    var fileInput = document.getElementById('AssignmentFile');
    if (fileInput) {
        fileInput.removeAttribute('required');
        fileInput.removeAttribute('data-val-required');
    }
});
document.getElementById('classSelect').addEventListener('change', function () {
    fetch('/ClassSubject/GetSubjectsForClass?classId=' + this.value)
        .then(response => response.json())
        .then(subjects => {
            var subjectSelect = document.getElementById('subjectSelect');
            subjectSelect.innerHTML = '';
            subjects.forEach(function (subj) {
                var opt = document.createElement('option');
                opt.value = subj.subjectId;
                opt.textContent = subj.name;
                subjectSelect.appendChild(opt);
            });
        });
});
// JS validation for file required only if selected
document.querySelector('form').addEventListener('submit', function (e) {
    var fileInput = document.getElementById('AssignmentFile');
    var fileError = document.querySelector('span[data-valmsg-for="AssignmentFile"]');
    if (fileInput && fileInput.value && !fileInput.value.toLowerCase().endsWith('.pdf')) {
        e.preventDefault();
        if (fileError) fileError.textContent = 'Only PDF files are allowed.';
        fileInput.classList.add('is-invalid');
    } else if (fileInput && fileInput.files && fileInput.files[0] && fileInput.files[0].size > 2 * 1024 * 1024) {
        e.preventDefault();
        if (fileError) fileError.textContent = 'File size must not exceed 2MB.';
        fileInput.classList.add('is-invalid');
    } else if (fileError) {
        fileError.textContent = '';
        fileInput.classList.remove('is-invalid');
    }
});