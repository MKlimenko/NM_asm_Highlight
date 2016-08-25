// Labels global description to access the routines from external C program
//global _Steps_1_3: label;
//global _Steps_4_8: label;
global _void._wht128_asm.8.8Steps_1_3.1unsigned._long._long._const._.0.9._unsigned._int.9._unsigned._long._long._.0.2:label;
global _void._wht128_asm.8.8Steps_4_8.1unsigned._long._long._const._.0.9._unsigned._long._long._.0.2:label;

// Section to allocate memory on global bus
nobits "dataGLOBAL"
global _GBuff: long[40h];
end "dataGLOBAL";

// Section to allocate memory on local bus
nobits "dataLOCAL"
global _LBuff: long[40h];
end "dataLOCAL";

// Section of weights to load into the Active Matrix Unit of NM6403
data ".data"
M_1_3: long[16] = ( 00001000100010001hl, 0FFFF0001FFFF0001hl,
0FFFFFFFF00010001hl, 00001FFFFFFFF0001hl,
00001000100010001hl, 0FFFF0001FFFF0001hl,
0FFFFFFFF00010001hl, 00001FFFFFFFF0001hl,
00001000100010001hl, 0FFFF0001FFFF0001hl,
0FFFFFFFF00010001hl, 00001FFFFFFFF0001hl,
0FFFFFFFFFFFFFFFFhl, 00001FFFF0001FFFFhl,
000010001FFFFFFFFhl, 0FFFF00010001FFFFhl);

end ".data";

// Section of source code.
begin ".text"
// The routine for the first 3 steps of FHT.

//<_Steps_1_3>
<_void._wht128_asm.8.8Steps_1_3.1unsigned._long._long._const._.0.9._unsigned._int.9._unsigned._long._long._.0.2>

.branch; // switch on the parallel vector instructions execution.

// Get position in stack to access the parametrs of the routine.
ar5 = ar7 - 2;

// Save registers
push ar0, gr0;
push ar1, gr1;
push ar4, gr4;

// Load the columns configuration of the Shadow Matrix.
gr1 = 80008000h;

push ar5, gr5;
// Copy the columns configuration into nb1 | Init gr4 with zero.
nb1 = gr1 with gr4 = false;

// Load the rows configuration of the Shadow Matrix.
sb = 03030303h;

// Load the address of weights array | gr4 = 1;
ar0 = M_1_3 with gr4++;

// Load 16 long words to wfifo, transfer 8 words to the Shadow Matrix
// and copy the Shadow Matrix contents to the Active Matrix.
rep 16 wfifo = [ar0++], ftw, wtw;

// Load the address of source buffer | gr4 = 2;
ar0 = [--ar5] with gr4 <<= 1;

gr0 = [--ar5];
// Load the address of destination buffer | gr5 = 4; <- Increment value
ar4 = [--ar5] with gr5 = gr4 « 1;
// ar5 points to the destination buffer address + 2 (next long word).
ar5 = ar4 + gr4 with gr4 = gr5;

// Load source data, make calculations and
// transfer 8 long words from wfifo to the Shadow Matrix.
rep 16 ram = [ar0++gr0],ftw with vsum , data, 0;
// Store results in memory. // These two instructions are executed
rep 16 [ar4++gr4] = afifo; // in parallel.

// This part of code is used to step over the silicon bug.
.wait; // switch off the parallel vector instructions execution.
// Copy the same constant to nb1 to lock wtw execution
// until ftw is finished.
nb1 = gr1;
// Copy the Shadow Matrix contents to the Active Matrix.
wtw;
.branch; // switch on the parallel vector instructions execution.

// Make a second part of calculations.
rep 16 with vsum , ram, 0; // These two instructions are executed
// Store results in memory. // in parallel.
rep 16 [ar5++gr5] = afifo; //

// Restore previous registers' values
pop ar5, gr5;
pop ar4, gr4;
pop ar1, gr1;
pop ar0, gr0;

// Return from the routine.
return;
.wait;

// The routine for the next 5 steps of FHT.

//<_Steps_4_8>
<_void._wht128_asm.8.8Steps_4_8.1unsigned._long._long._const._.0.9._unsigned._long._long._.0.2>

.branch; // switch on the parallel vector instructions execution.

// Get position in stack to access the parametrs of the routine.
ar5 = ar7 - 2;

// Save registers
push ar0, gr0;
push ar1, gr1;
push ar2, gr2;
push ar4, gr4;
push ar5, gr5;
push ar6, gr6;

// Load the address of weights array | Init gr0 with zero.
ar6 = M_4_8 with gr0 = false;

// Load the columns configuration of the Shadow Matrix.
gr2 = 80008000h;

// Load the rows configuration of the Shadow Matrix.
sb = 022222222h;

// Copy the columns configuration into nb1 | gr0 = 1;
nb1 = gr2 with gr0++;

// Load the address of source buffer | gr0 = 2;
ar0 = [--ar5] with gr0 «= 1;

// ar1 points to the source buffer address + 2 (next long word)| gr1 = 4
ar1 = ar0 + gr0 with gr1 = gr0 « 1;

// Load the address of destination buffer | gr4 = 2;
ar4 = [--ar5] with gr4 = gr0;

// ar1 points to the destination buffer address + 2 | gr5?? = 4
ar5 = ar4 + gr4 with gr5 = gr1;

// gr0 = 4 | gr4 = 4 <- Increments to access odd/even long words.
gr0 = gr1 with gr4 = gr1;

// Load source data to wfifo -> ShM -> ActM
rep 16 wfifo = [ar0++gr0], ftw, wtw;
// Load weights to ram; // These two instructions are executed
rep 16 ram = [ar6++]; // in parallel.

// Load next part of source data to wfifo -> ShM | Make calculations.
rep 16 wfifo = [ar1++gr1], ftw with vsum , ram, 0;
// Store results in memory. // These two instructions are executed
rep 16 [ar4++gr4] = afifo; // in parallel.

// This part of code is used to step over the silicon bug.
.wait; // switch off the parallel vector instructions execution.
// Copy the same constant to nb1 to lock wtw execution
// until ftw is finished.
nb1 = gr2;
// Copy the Shadow Matrix contents to the Active Matrix.
wtw;
.branch; // switch on the parallel vector instructions execution.

// Make calculations. //
rep 16 with vsum , ram, 0; // These two instructions are executed
// Store results in memory. // in parallel.
rep 16 [ar5++gr5] = afifo; //

// Restore previous registers' values
pop ar6, gr6;
pop ar5, gr5;
pop ar4, gr4;
pop ar2, gr2;
pop ar1, gr1;
pop ar0, gr0;

// Return from the routine.
return;
.wait;

end ".text";