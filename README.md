# Authors

* Tony CLONIER (tony.clonier)
* Rodolphe GUILLAUME (guilla_w)

# How to use

Nous avons fait le choix d'une interface en ligne de commande (CLI).
Nous �tions parti sur un Windows Form, mais �tant tous deux sous UNIX, il nous �tait difficile de continuer seuls dans cette direction.

La CLI permet de rechercher des bagages via leur code IATA, et d'en cr�er un dans le cas o� la recherche ne renvoie aucun r�sultat.

## Recherche d'un bagage

Pour rechercher un bagage, il suffit de taper son code IATA. Il vous est alors retourn� sous la forme d'une cha�ne de caract�res.

![](./misc/one-bagage.png)

## Recherche de plusieurs bagages

La recherche d'une liste de bagages est similaire � la recherche d'un bagage unique.
Lorsqu'un code IATA correspond � plusieurs bagages, la liste de ceux-ci est affich�e.

A noter �galement que la recherche s'effectue en regardant quels codes IATA ou morceaux de codes IATA correspondent � l'entr�e de l'utilisateur.

> Par exemple, la recherche "42" pourrait correspondre aux codes IATA "42000000", "00420000" ou bien encore "00000042".

![](./misc/list-bagages.png)

## Ajout d'un bagage

L'ajout d'un bagage s'enclenche lorsqu'une recherche ne donne aucun r�sultat.

L'utilisateur est alors invit� � rentrer les informations concernant son bagage.

![](./misc/insert-bagage.png)

On peut alors v�rifier l'insertion avec des visualisateurs de bases de donn�es, comme SSMS.

![](./misc/query.PNG)
![](./misc/query-result.PNG)

Cependant, le bagage entr� n'appara�t pas dans les recherches des utilisateurs.
Nous pensons que cela vient du fait que l'insertion dans la table BAGAGE n'est pas suffisante, et que des cl�s �trang�res devraient �tre ins�r�es dans d'autres tables.

![](./misc/not-found)