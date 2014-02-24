:: Removes folders from previous builds 
FOR /D %%X IN (Output_*) DO RD /S /Q "%%X"