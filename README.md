# GZipTest
Test task for the position of c# developer at Veeam. Block-by-block compression and decompression files in a multiprocessor environment.
#
Console application c# for multitasking compression and decompression of files.
 - One Task performs the function of reading the file in blocks of 1 megabyte and writing to the queue, as an index and a data array.
 - Create Tasks that takes the initial data from the queue, transform it and write the result into the dictionary, as an initial index and a transform data array.
 - One Task performs the function of writing result data to the file. It takes by index data from dictionary and write to the file.

Block-by-block compression of files inqlude writing the length of the compressed block to the header after the Gzip key bytes. 
Block-by-block decompression of files inqlude reading the length of the compressed block from the header after the Gzip key bytes. 
