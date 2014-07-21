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

                var eventObj = new Object();
                eventObj.id = data[event_Obj].Id;
                eventObj.data = data[event_Obj].DataPubblicazione;
                eventObj.titolo = data[event_Obj].Titolo;
                eventObj.testo = data[event_Obj].Testo;
                eventObj.foto = data[event_Obj].UrlFoto;

                var eventObjLine = "<tr>"
                                + "<td class='id'>" + eventObj.id + "</td>"
                                + "<td><b>" + eventObj.titolo + "</b></td>"
                                + "<td>" + eventObj.data + "</td>"
                                + "<td><img src='" + eventObj.foto + "' class='image' style='width:150px; height:auto;' /></td>"
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
                    url: '../api/events/' + id,
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
                            if ($('#file').val() == "") {
                                obj.UrlFoto = data.UrlFoto;
                            } else {
                                obj.UrlFoto = $('#file').val();
                            }

                            $.ajax({
                                type: 'PUT',
                                url: '../ITSAppFinale/api/events',
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