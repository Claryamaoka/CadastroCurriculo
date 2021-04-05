document.querySelector("#add-idiom").addEventListener('click', unHiddenIdiom)

document.querySelector("#add-course").addEventListener('click', unHiddenStudies)

document.querySelector("#add-profession").addEventListener('click', unHiddenProfession)

$("#select-cep").change(function () {
    //debugger;
    var valor = $("#select-cep option:selected").val();
    $.ajax({
        method: "POST",
        url: "/home/RetornaEndereco",
        data:{ cep: valor },
        //dataType: 'json',
        //contentType: false,
        //processData: false,
        success: function (response) {
            //debugger;
            if (response.sucesso === true) {
                //debugger;
                $("#street").val(response.street);
                $("#state").val(response.state)
                $("#city").val(response.city)
                $("#district").val(response.district)
                $("#number").val(response.number)
            } else {
                //debugger;
            }

        },
        error: function (response) {
            
        }
    });
});


//executa a ação de clonar os campos
function cloneFieldIdiom() {
    var doc = document.getElementById('schedule-idiom').children.length;
    if (doc == 5)
        return;

    const newFieldContainer = document.querySelector('.item-idiom').cloneNode(true)

    //limpar os campos
    const fields = newFieldContainer.querySelectorAll('input');

    //para cada campo limpar
    fields.forEach(function (field) {
        //pegar o field do momento e limpá-lo
        field.value = ""
    })

    
    //colocar na página
    document.querySelector('#schedule-idiom').appendChild(newFieldContainer)
}

function cloneFieldProfession() {
    var doc = document.getElementById('schedule-profession').children.length;
    if (doc == 5)
        return;
    const newFieldContainer = document.querySelector('.item-profession').cloneNode(true)

    //limpar os campos
    const fields = newFieldContainer.querySelectorAll('input')

    //para cada campo limpar
    fields.forEach(function (field) {
        //pegar o field do momento e limpá-lo
        field.value = ""
    })

    //colocar na página
    document.querySelector('#schedule-profession').appendChild(newFieldContainer)
}

function cloneFieldStudies() {
    var doc = document.getElementById('schedule-course').children.length;
    if (doc == 5)
        return;
    const newFieldContainer = document.querySelector('.item-course').cloneNode(true)

    //limpar os campos
    const fields = newFieldContainer.querySelectorAll('input')

    //para cada campo limpar
    fields.forEach(function (field) {
        //pegar o field do momento e limpá-lo
        field.value = ""
    })

    //colocar na página
    document.querySelector('#schedule-course').appendChild(newFieldContainer)
}


function unHiddenIdiom() {
    var f = document.querySelector('#idiom-one');
    var j = document.querySelector('#idiom-two');
    if (f.hasAttribute("hidden"))
        f.removeAttribute("hidden");
    else
        j.removeAttribute("hidden");
}

function unHiddenProfession() {
    var f = document.querySelector('#profession-one');
    var j = document.querySelector('#profession-two');
    if (f.hasAttribute("hidden"))
        f.removeAttribute("hidden");
    else
        j.removeAttribute("hidden");
}


function unHiddenStudies() {
    var f = document.querySelector('#course-one');
    
    var j = document.querySelector('#course-two');
    if (f.hasAttribute("hidden"))
        f.removeAttribute("hidden");
    else
        j.removeAttribute("hidden");
}

