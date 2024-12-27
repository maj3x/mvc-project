// Global AJAX setup
$.ajaxSetup({
    headers: {
        'RequestVerificationToken': $('input:hidden[name="__RequestVerificationToken"]').val()
    }
});

// DataTables Türkçe dil ayarı
$.extend(true, $.fn.dataTable.defaults, {
    language: {
        url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/tr.json'
    },
    responsive: true
});

// Tarih formatı için yardımcı fonksiyon
function formatDate(date) {
    return new Date(date).toLocaleDateString('tr-TR', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
}

// Form validasyonu için özel kurallar
$.validator.addMethod("turkishPhone", function(value, element) {
    return this.optional(element) || /^(\+90|0)?[0-9]{10}$/.test(value);
}, "Lütfen geçerli bir telefon numarası giriniz");

// Toastr ayarları
toastr.options = {
    closeButton: true,
    progressBar: true,
    positionClass: "toast-bottom-right",
    timeOut: 5000
};

// Bootstrap Tooltip aktivasyonu
$(function () {
    $('[data-bs-toggle="tooltip"]').tooltip();
});

// Form submit öncesi kontrol
$(document).on('submit', 'form', function() {
    $(this).find('button[type="submit"]').prop('disabled', true);
    return true;
});

// AJAX hata yönetimi
$(document).ajaxError(function(event, jqxhr, settings, thrownError) {
    toastr.error('Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.');
});

// Status badge renk sınıfları
function getBadgeClass(status) {
    switch (status) {
        case 'Completed': return 'bg-success';
        case 'InProgress': return 'bg-primary';
        case 'Late': return 'bg-danger';
        default: return 'bg-warning';
    }
}

// Sayfa yüklendiğinde çalışacak genel fonksiyonlar
$(document).ready(function() {
    // DataTables inicializasyonu
    $('.table').DataTable({
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/tr.json'
        },
        responsive: true
    });

    // Select2 inicializasyonu
    $('.select2').select2({
        language: "tr"
    });

    // Form validasyon mesajlarının Türkçeleştirilmesi
    $.extend($.validator.messages, {
        required: "Bu alan zorunludur.",
        email: "Lütfen geçerli bir e-posta adresi giriniz.",
        minlength: "Lütfen en az {0} karakter giriniz.",
        maxlength: "Lütfen en fazla {0} karakter giriniz.",
        equalTo: "Lütfen aynı değeri tekrar giriniz.",
        remote: "Lütfen bu alanı düzeltiniz.",
        date: "Lütfen geçerli bir tarih giriniz.",
        dateISO: "Lütfen geçerli bir tarih giriniz (ISO).",
        number: "Lütfen geçerli bir sayı giriniz.",
        digits: "Lütfen sadece sayısal değerler giriniz.",
        creditcard: "Lütfen geçerli bir kredi kartı numarası giriniz.",
        accept: "Lütfen geçerli bir değer giriniz.",
        pattern: "Lütfen geçerli bir format giriniz."
    });
});
