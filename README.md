# Introduction
ScanOpener est un logiciel qui simplifie la tâche des gens qui ont à ouvrir
des documents spécifiques selon chaque type de pièce qu'ils ont à traiter.

Plus concrètement, supposons une usine d'assemblage : quand un ouvrier reçoit
la commande de produire une pièce d'un certain type il doit ouvrir le 
répertoire correspondant à la pièce puis y lire quelques documents pdf 
(notice de montage, plan d'assemblage, etc.). Dans la pratique il doit naviger
manuellement sur un écran d'affichage pour trouver le repertoire portant le
bon numéro d'item, puis ouvrir les bons fichiers. Ces opérations sont 
fastidieuses pour les opérateurs et comportent un fort risque d'erreur.

Avec scanOpener, on peut simplifier ces opérations. À la base on suppose que
les documents de référence pour un type de pièce sont placés dans un 
répertoire portant le nom (ou le numéro) de ce type. Tous ces répertoires sont
dans un répertoire commun. À l'aide d'un lecteur de code à barre, l'opérateur
indique le type de pièce à produire. Grâce à une table de conversion (code à
barre -> code de pièce), scanOpener scanOpener determine le répertoire à 
ouvrir et pourra l'ouvrir automatiquement dans une fenêtre explorer. Il peut
être aussi configuré pour ouvrir les documents s'y trouvant.

# Préequis
## Installation du logiciel
Ce logiciel tourne sous la plateforme .net 4.0 et ne nécessite que son ficher
.exe pour fonctionner. L'installation peut donc se résumer à copier
l'application sur un ordinateur.

L'administrateur devra configurer l'application grâce à son dialogue des
paramètres et devra probablement fournir une table pour convertir les codes
à barre lus dans leur type de pièce équivalent.

## Lecteur de code à barre
Bien que le logiciel puisse fonctionner avec un clavier, il prend tout son
intérêt avec un lecteur de code à barre. On utilise un lecteur qui transmet
ses informations sur le port série (COM).
