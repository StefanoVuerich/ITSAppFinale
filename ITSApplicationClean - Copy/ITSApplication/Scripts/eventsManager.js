$(function () {
    var myModal = $('#createNewModal');
    $('#createNewArticle').on('click', function () {
        myModal.modal();
        $('#file').change(function () {
            readURL(this);
        });
    });
    $.ajax({
        url: '../api/events',
        type: 'GET',
        datatype: "json",
        success: function (data) {

            for (var event_Obj in data) {
                var testo = data[event_Obj].Testo;
                var preTesto = testo.substring(0, 500);
                var foto = data[event_Obj].UrlFoto;
                if (foto = "") {
                    foto = "";
                } else {
                    foto = "<img src='" + data[event_Obj].UrlFoto + "' class='image' style='width:150px; height:auto;' />"
                }

                var eventObjLine = "<tr>"
                                + "<td class='id'>" + data[event_Obj].Id + "</td>"
                                + "<td>" + data[event_Obj].DataPubblicazione + "</td>"
                                + "<td><b>" + data[event_Obj].Titolo + "</b></td>"
                                + "<td>" + preTesto + "</td>"
                                + "<td>" + foto + "</td>"
                                + "<td><span class='glyphicon glyphicon-edit editEvent'></span><br/><span class='glyphicon glyphicon-trash deleteEvent'></span</td>"
                                + "</tr>";

                $('#listaEvents').prepend(eventObjLine);
            }
            $('.image').on('click', function () {
                var obj = $(this);
                $('#imageModal').modal();
                $('#image').attr('src', obj.attr('src'));
            });

            $('.deleteEvent').on('click', function () {

                var obj = $(this);
                var id = obj.parent().siblings('.id').text();

                $.ajax({
                    type: 'DELETE',
                    url: '../api/events/del/' + id,
                    contentType: 'json',
                    dataType: 'json',

                    success: function (data) {
                        console.log("dato cancellato");
                        obj.parent().parent('tr').remove();
                    },
                    error: function (xhr) {
                        console.log('Error in Operation');
                    }
                });
            });

            $('.editEvent').on('click', function () {
                var id = $(this).parent().siblings('.id').text();
                $.ajax({
                    type: 'GET',
                    url: '../api/event/' + id,
                    contentType: 'json',
                    dataType: 'json',
                    success: function (data) {

                        myModal.modal();
                        $('#titolo').val(data.Titolo);
                        $('#testo').val(data.Testo);
                        $('#previewHolder').attr('src', data.UrlFoto);
                        $('#file').change(function () {
                            readURL(this);
                        });

                        $('#save').on('click', function (e) {
                            e.preventDefault();
                            var obj = new Object();
                            obj.Id = data.Id;
                            obj.DataPubblicazione = data.DataPubblicazione;
                            obj.Titolo = $('#titolo').val();
                            obj.Testo = $('#testo').val();
                            obj.UrlFoto = "";

                            var formdata = new FormData();
                            //var image = document.getElementById('file');
                            var fileInput = document.getElementById('file');
                            //Iterating through each files selected in fileInput
                            for (i = 0; i < fileInput.files.length; i++) {
                                //Appending each file to FormData object
                                formdata.append(obj.Titolo, fileInput.files[i]);
                            }

                            //formdata.append(image.files.name, image.files);

                            if ($('#file').val() == "") {
                                obj.UrlFoto = data.UrlFoto;
                            } else {
                                obj.UrlFoto = data.UrlFoto;
                                var xhr = new XMLHttpRequest();
                                xhr.open('PUT', '/Form/EditEntity');
                                xhr.send(formdata);
                                xhr.onreadystatechange = function () {
                                    if (xhr.readyState == 4 && xhr.status == 200) {
                                        alert(xhr.responseText);
                                        $.ajax({
                                            type: 'PUT',
                                            url: '../api/events/',
                                            data: JSON.stringify(obj),
                                            contentType: 'application/json; charset=utf-8',
                                            dataType: 'json',
                                            processData: true,
                                            success: function (obj, status, jqXHR) {
                                                //alert('success...' + data);
                                                myModal.modal('hide');
                                                location.reload();
                                            },
                                            error: function (xhr) {
                                                alert(xhr.responseText);
                                            }
                                        });
                                    }
                                }
                            }
                        });

                    },
                    error: function (xhr) { }
                });
            });

        }, error: function (data) {
            console.log('Error in Operation');
        }
    });
    $('#EsciSenzaSalvare').on('click', function () {
        myModal.modal('hide');
        $('#titolo').val('');
        $('#testo').val('');
        $('#file').val('');
        $('#previewHolder').attr('src', '');
    });
});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#previewHolder').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}