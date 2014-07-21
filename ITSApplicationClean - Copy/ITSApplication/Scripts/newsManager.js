$(function () {
    var myModal = $('#createNewModal');
    $('#createNewArticle').on('click', function () {
        myModal.modal();
        $('#file').change(function () {
            readURL(this);
        });
    });
    $.ajax({
        url: '../api/news',
        type: 'GET',
        datatype: "json",
        success: function (data) {
            for (var news_obj in data) {
                var news = new Object();
                news.id = data[news_obj].Id;
                news.data = data[news_obj].DataPubblicazione;
                news.titolo = data[news_obj].Titolo;
                news.testo = data[news_obj].Testo;
                news.foto = data[news_obj].UrlFoto;
                var newsObj = "<tr>"
                                + "<td class='id'>" + news.id + "</td>"
                                + "<td class='titolo'><b>" + news.titolo + "</b></td>"
                                + "<td class='data'>" + news.data + "</td>"
                                + "<td><img src='" + news.foto + "' class='image' style='width:150px; height:auto;' /></td>"
                                + "<td><span class='glyphicon glyphicon-edit editNews'></span><br/><span class='glyphicon glyphicon-trash deleteNews'></span></td>"
                                + "</tr>";
                $('#listaNews').prepend(newsObj);
            }
            $('.image').on('click', function () {
                var obj = $(this);
                $('#imageModal').modal();
                $('#image').attr('src', obj.attr('src'));
            });

            $('.deleteNews').on('click', function () {
                var obj = $(this);
                var id = obj.parent().siblings('.id').text();;
                $.ajax({
                    type: 'DELETE',
                    url: '../api/news/' + id,
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
            $('.editNews').on('click', function () {
                var id = $(this).parent().siblings('.id').text();
                $.ajax({
                    type: 'GET',
                    url: '../api/news/' + id,
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
                                url: '../ITSAppFinale/api/news',
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