# VRBike
Bicicleta VR

Versão Unity: 2022.3.5f1

> [!NOTE]
> Para alterar o tamanho da cidade, basta alterar a escala no Transform do gerador da cidade (CityGenerator). Isso deve redimensionar a cidade inteira,
> incluindo carros, postes, árvores e os prédios/casas. Embora só a escala do eixo X seja relevante no código, favor, redimensionar os três eixo igualmente.

Para poder movimentar a bicicleta, adicionar o script MovePlaferWithMovementSource a esta. Em seguida, deve-se
adicionar o conjunto de controladores que se deseja utilizar ao componente (mesmo que se deseje utilizar um só, o controlador deverá ser
um objeto filho de outro objeto).

> [!NOTE]
> O script pode ser adicionado a qualquer objeto se assim desejar. Na cena FullSetup, ele foi
> adicionado ao objeto `XR Origin (XR Rig)` (e o script VrFollowBicycle foi adicionado à bicicleta) para que não
> haja _jittering_ no modelo da bicicleta e para que o movimento do jogador seja tão suave quanto possível (
> fica meio "travado" quando o jogador "segue" a bicicleta).

A escolha de qual controlador utilizar se dá pela busca do primeiro que estiver ativo durante a execução. Outros
controladores ativos serão ignorados.

Para criar seu próprio controlador, basta criar um novo script que implemente a interface IBicycleMovementSource. O script
adicionado à bicicleta deve ser capaz de ler os dados gerados pelo novo controlador e movimentar a bicicleta sem problemas
(contanto que os dados estejam sendo gerados e tratados corretamente dentro do próprio controlador).

O vetor de direção retornado pela função GetFrontWheelDirection, parte da interface IBicycleMovementSource, não é utilizado
pelo script que move a bicicleta. Recomenda-se, entretanto, que esse vetor se comporte da maneira esperada indicada pelo nome da função.
Isto é, deve-se retornar um vetor que aponte para a frente do pneu frontal (ou para a frente do guidão se assim preferires).

## Fontes de Movimento (Controladores)

### BicycleSimpleMovementSource.cs

Controla a direção e a velocidade da bicicleta, com aceleração ao pedalar e ao não pedalar/frear.

É possível controlar como, exatamente, a direção é gerenciada. Se `simplerSteering` estiver habilitado,
o controlador utilizará dos valores do vetor recebido em OnMove para controlar a direção. Caso contrário,
será necessário pressionar repetidas vezes os botões direcionais para alterar a direção.

Recomenda-se habilitar `simplerSteering` ao testar a bicicleta com controles analógicos (este controlador lida melhor com analógicos).

### BicycleExternalMovementSource.cs [incompleto]

Deveria controla tanto a direção quanto a velocidade, mas este controlador encontra-se incompleto (não é utilizado).

O propósito deste controlador é utilizar um potenciômetro conectado a um Arduino para controlar a direção da bicicleta.
A velocidade, entretanto, é constante, definida diretamente no código do controlador.

### BicycleForwardOnlyMovementSource.cs

Controla somente a velocidade da bicicleta, com aceleração, mas sem controle de direção. Apenas isso.

### BicycleBetterMovementSource.cs

Controla a velocidade, com aceleração, e a direção da bicicleta de maneira suave.

Feito para ser melhor de controlar do que o BicycleSimpleMovementSource.cs quando testado com um teclado.
Funciona com controles analógicos também, mas não é ideal.

### BicycleUDPMovementSource.cs [WIP]

Controla velocidade e direção da bicicleta. Assim como o nome implica, UDP faz parte desse processo.

Este controlador, por meio de sockets UDP, recebe informações de sensores acoplados a uma bicicleta real e traduz os valores
desses sensores de maneira que a bicicleta possa ser controlada adequadamente. Os sensores em questão são: um giroscópio para
controlar a direção e uma câmera (ou outro sensor qualquer) que consiga capturar a velocidade da roda traseira da bicicleta.

Por conta do movimento vir
de fora da Unity Engine, este controlador não trata aceleração nem suavidade das curvas, sendo estas dependentes da bicicleta real
e dos sensores.

As portas utilizadas pelos sockets podem ser alteradas. O formato esperado dos valores recebidos do giroscópio é o seguinte:
`x;y;z`.

Por enquanto encontra-se incompleto.
