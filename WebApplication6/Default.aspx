<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Registro Exitoso</title>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script>
        document.addEventListener("DOMContentLoaded", function() {
        let timerInterval;
            Swal.fire({
            icon: "success",
            title: "¡Registro Exitoso!",
            html: "Esta Ventana se cerrará <b></b> milisegundos.",
            timer: 1800,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
                const timer = Swal.getPopup().querySelector("b");
                timerInterval = setInterval(() => {
                    timer.textContent = `${Swal.getTimerLeft()}`;
                }, 100);
            },
            willClose: () => {
                clearInterval(timerInterval);
            }
        }).then(() => {
                window.location.href = "Asistencia.aspx?idcliente="; // Cambia por la URL de tu página principal
        });
        });

    </script>
</head>
<body>
    <!-- No se necesita contenido en el cuerpo de la página -->
</body>
</html>
