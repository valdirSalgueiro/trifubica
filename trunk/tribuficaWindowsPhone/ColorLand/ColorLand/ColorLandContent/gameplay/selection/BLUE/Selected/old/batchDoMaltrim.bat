FOR %%a in (*.png) DO convert %%a -trim -flatten +repage %%a
pause