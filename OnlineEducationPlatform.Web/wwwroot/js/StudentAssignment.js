
document.addEventListener('DOMContentLoaded', function () {
    var submitButtons = document.querySelectorAll('.submit-assignment-btn');
    var modal = new bootstrap.Modal(document.getElementById('submitAssignmentModal'));
    var form = document.getElementById('assignmentSubmissionForm');
    var submitBtn = document.getElementById('submitAssignmentBtn');
    var fileInput = document.getElementById('submissionFile');
    var fileError = document.getElementById('submissionFileError');
    var errorAlert = document.getElementById('submissionError');
    var successAlert = document.getElementById('submissionSuccess');

    submitButtons.forEach(function (btn) {
        btn.addEventListener('click', function () {
            var assignmentIdInput = document.getElementById('modalAssignmentId');
            if (assignmentIdInput) {
                assignmentIdInput.value = btn.getAttribute('data-assignment-id');
            }
            var assignmentTitleInput = document.getElementById('modalAssignmentTitle');
            if (assignmentTitleInput) {
                assignmentTitleInput.value = btn.getAttribute('data-title');
            }
            var assignmentDescriptionInput = document.getElementById('modalAssignmentDescription');
            if (assignmentDescriptionInput) {
                assignmentDescriptionInput.value = btn.getAttribute('data-description');
            }
            fileInput.value = '';
            fileError.textContent = '';
            errorAlert.classList.add('d-none');
            successAlert.classList.add('d-none');
            modal.show();
        });
    });

    submitBtn.addEventListener('click', function () {
        var form = document.getElementById('assignmentSubmissionForm');
        var formData = new FormData(form);
        // Ensure AssignmentId is set
        formData.set('AssignmentId', document.getElementById('modalAssignmentId').value);
        // Ensure anti-forgery token is set
        var token = document.querySelector('#assignmentSubmissionForm input[name="__RequestVerificationToken"]').value;
        formData.append('__RequestVerificationToken', token);
        fetch('/StudentAssignment/Submit', {
            method: 'POST',
            body: formData
        })
        .then(response => response.json())
        .then(data => {
            console.log(data);
            if (data.success) {
                successAlert.textContent = 'Assignment submitted successfully!';
                successAlert.classList.remove('d-none');
                setTimeout(() => { window.location.reload(); }, 1200);
            } else {
                errorAlert.textContent = data.error || 'Submission failed.';
                errorAlert.classList.remove('d-none');
            }
        })
        .catch(() => {
            errorAlert.textContent = 'An error occurred while submitting.';
            errorAlert.classList.remove('d-none');
        });
    });
});

