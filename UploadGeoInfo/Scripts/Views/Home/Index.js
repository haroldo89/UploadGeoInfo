/**
 * Servicio de direcciones y calculo en porcentaje de ruta.
 */
function UploadGeoFence() { }

UploadGeoFence.map = null;

UploadGeoFence.CurrentUploadedFileName = null;


UploadGeoFence.getPointsLocationsData = function () {
    var principalCitiesOfColombia = [
  		['Bogota', 4.3400, -74.0000, 1],
  		['Cali', 3.2500, -76.3500, 2],
  		['Pereira', 4.6750, -75.4300, 3],
  		['Medellin', 6.1500, -75.3500, 4],
  		['Bucaramanga', 7.0000, -73.0000, 5],
  		['Cartagena', 10.3550, -75.5000, 6]
    ];
    return principalCitiesOfColombia;
};

/**
 * Limpiar controles
 * 
 * @returns {void}
 */
UploadGeoFence.clearMapControls = function () {
    UploadGeoFence.initMap();
    return false;
}

/**
 * Inicializar componentes.
 */
UploadGeoFence.initializeComponents = function () {
    // unblock when ajax activity stops 
    UploadGeoFence.clearMapControls();
    UploadGeoFence.initializeUploadFile();
};

/**
 * Inicializar mapa
 */
UploadGeoFence.initMap = function () {
    // Opciones de mapa.
    var mapOptions = {
        zoom: 5,
        center: { lat: 4.6750, lng: -75.4300 },
        scaleControl: true
    }
    // Contenedor del mapa
    var containerMap = document.getElementById('map');
    // Crear mapa.
    UploadGeoFence.map = GoogleMapsHelper.initializeMap(containerMap, mapOptions);
}


UploadGeoFence.initializeUploadFile = function () {
    //Inicializa el control para subir archivos
    $('#fileUpload').fileupload({
        dataType: 'json',
        url: '/Home/UploadRouteFile',
        autoUpload: true,
        done: function (e, data) {
            UploadGeoFence.initializeUploadFile_onDone(e, data);
        }
    }).on('fileuploadprogressall', function (e, data) {
        $("#spinnerRouteImportMap").show();
        UploadGeoFence.initializeUploadFile_onProgress(e, data);
    });
};

UploadGeoFence.initializeUploadFile_onDone = function (e, data) {
    if (data.result.error == null) {
        var fileName = data.result.uploadFileResult.name;
        UploadGeoFence.CurrentUploadedFileName = fileName;
        UploadGeoFence.getPreviewRouteFileData(fileName);
    }
    else {
        $("#spinnerRouteImportMap").hide();
        MessageBox.show(data.result.error)
    }
};

UploadGeoFence.initializeUploadFile_onProgress = function (e, data) {
    //UploadGeoFence.initializeControls();
    var progress = parseInt(data.loaded / data.total * 100, 10);
    UploadGeoFence.setPercentProgressValue(progress);
};

UploadGeoFence.getPreviewRouteFileData = function (fileName) {
    $.ajax({
        url: "/Home/GetPreviewRouteFileData",
        cache: false,
        data: {
            fileName: fileName
        },
        success: function (result) {
            UploadGeoFence.getPreviewRouteFileData_onSuccess(result);
        },
        error: function (request, status, error) {
            UploadGeoFence.getPreviewRouteFileData_onError(request, status, error);
        }
    });
}

UploadGeoFence.getPreviewRouteFileData_onSuccess = function (result) {
    if (result.errorMessage == null) {
        $('#divRouteFileUploadMapContent').html(result);
        LayoutHelper.setVisible('btnSaveRoute', true);
        var totalElements = $("#hdnTotalElements").val();
        $("#lblTotalElements").text(String.format("Total: {0} Registros", totalElements));
    }
    else {
        UploadGeoFence.initializeControls();
        MessageBox.show(result.errorMessage);
    }
}

UploadGeoFence.getPreviewRouteFileData_onError = function (request, status, error) {
    $("#spinnerRouteImportMap").hide();
    console.error("Error desconocido en generacion de vista previa de datos de archivo csv de puntos de control");
}

UploadGeoFence.setPercentProgressValue = function (value) {
    if (value >= 0 && value <= 100) {
        $('.progress .progress-bar').css('width', value);
    }
}