Archivos CUE

Este proyecto inició en el año 2014, hecho en C# y WPF, su versión original se borró accidentalmente en un formateo luego de respaldar mal los archivos.
Esta versión del proyecto comprende el desarrollo del programa desde cero, cambiando la forma en que se utiliza, pero cumpliendo la misma tarea.

Acerca del programa:
El creador de archivos CUE es un programa que le pide al usuario toda la información necesaria acerca de un disco de audio (Título del disco, Artista del disco, las pistas y sus índices) utilizando también el nombre del archivo de audio fuente (WAV o FLAC) para crear un archivo CUE que pueda ser leído por programas de grabación como ImgBurn o por programas de unidades virtuales como Daemon Tools.

Uso del programa:

La primera versión del programa contenía una versión simplificada del registro:
- Un textbox para el título del álbum.
- Un textbox para el artista del álbum.
- Un textbox para el género.
- Un textbox para el año.
- Un textbox para el número de pistas.
- Un botón para generar las pistas.
- Un textbox para el título de la pista.
- Un textbox para el artista de la pista.
- Un textbox para el "INDEX 00". (Opcional)
- Un textbox para el "INDEX 01".
- Un combobox para seleccionar la pista que se desea editar.
- Un textbox para escribir el nombre del archivo de audio fuente.
- Un botón para abrir y obtener el nombre del archivo de audio fuente.
- Un botón para guardar la información de la pista.
- Un botón para guardar el archivo CUE.
- Un botón de recomendación.
- Un botón para limpiar todos los campos.
- Un botón para salir del programa.

El usuario primero abre el programa, y procede a ingresar el título del álbum, el artista, el género, el año y el número de pistas (en esta versión, el género y el año eran requeridos). Luego de ingresar todos esos datos, el usuario procede a "Generar" las pistas, luego de eso, el combobox de las pistas se rellenaba del 1 hasta n (siendo n el número total de pistas). El usuario rellena los campos que tienen que ver con la información de la pista, lo normal es hacerlo en orden, desde la primera pista. El usuario ingresa el título de la pista, el artista, el "INDEX 00" (lo cual era opcional, y solo debía usarse si el usuario entendía para qué era) y el "INDEX 01" (el momento donde inicia la pista). El usuario debe ingresar estos dos datos bajo el formato "MM:SS:FF" (MM = Minutos; SS = Segundos; FF = Frames o Cuadros), las restricciones de este formato es que los Cuadros (FF) deben ser entre 00 y 74, y los Segundos (SS) deben ser entre 00 y 59. Luego de ingresar estos datos, se procede a buscar el archivo de audio fuente, éste sólo puede ser WAV o FLAC. Teniendo todo esto, el usuario guarda la información de la pista haciendo clic en el botón para guardar la información, si no lo hace, el usuario perdería la información que ingresó al cambiar de pista. Luego de ingresar la primera pista, el usuario repite el proceso para el resto de las pistas. Por defecto, todas las pistas tiene el archivo fuente de audio como vacío, por lo tanto, si se cambia a una pista nueva, los únicos campos que se borrarán serán Título, Artista, INDEX 00 e INDEX 01, si el archivo fuente es vacío para la siguiente pista, el nombre del archivo fuente se mantiene, sin embargo, no está registrado oficialmente, para eso es necesario hacer clic en el botón "Guardar Pista". Una vez que todas las pistas hayan sido configuradas, el usuario procede a "Guardar" el archivo CUE, de preferencia, para que no existan problemas en el uso del archivo, se debe guardar el archivo CUE donde se encuentra(n) el(los) archivo(s) fuente(s). Si todos los datos ingresados son correctos, se prepara el archivo y se guarda con el nombre que el usuario eligió en la ruta escogida. Después de guardar el archivo, el usuario puede limpiar todos los campos para crear otro archivo CUE para otro álbum, o cerrar el programa. Adicionalmente, existe un botón que muestra un mensaje indicando que, para que el uso del archivo CUE en un programa como Daemon Tools fuera exitoso, se requiere usar un archivo FLAC o WAV con 16 bits de profundidad, y una frecuencia de 44.1 kHz, ni más ni menos.


Comparación con la nueva versión:

