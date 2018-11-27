# tp-dotnet-apping Poste Identification Manuel PIM

# Architecture du projet

## Models

* ModelAbstractDefinition = definit ce que doit retourner un modele
* NodelNatif = objet qu'on creer a la main
* ModelSQL = entite de notre DB
* ModelFactory = on decrit la DB sur laquelle on se co (Oracle, Postgres)
* ModelNatif, ModelSQL = implementation de mon ModelAbstractDefinition

## Classe abstraite vs Interface ?

Classe abstraite garde la logique objet, avec l'heritage etc.
L'interface c'est surtout pour donner une fonctionnalite en plus.

sealed = final
