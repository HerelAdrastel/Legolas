#!/usr/bin/env python3

import os

root = "/home/herel/UnityProjects/Legolas"

lines = 0
caracteres = 0

for path, subdirs, files in os.walk(root):
	for name in files:
		if name.endswith((".cs", ".meta")):
			pathfile = os.path.join(path, name)
			
			with open(pathfile, "r") as file:
				linesObj = file.readlines()
				
				lines += len(linesObj)
				
				for line in linesObj:
					caracteres += len(line)


print("Wow ton application possède " + str(lines) + " lignes de codes avec " + str(caracteres) + " caractères")
