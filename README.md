# OCTO Api Client

O **OCTO.Api.Client** � uma biblioteca que foi criada para servir como base no consumo das APIs de Integra��o do MasterSGi e SGFlex.

Dentro do projeto, voc� encontrar� a Classe **OCTO.Api.Client.ApiRepository** que � a implementa��o no modelo Repository para acessar as APIs de Integra��o.

No construtor da **ApiRepository** voc� deve informar o **AuthType** que � definido pela Enum: **OCTO.Api.Client.AuthType**.

O M�todo de Autentica��o utilizado atualmente tanto pelo **MasterSGi** como pelo **SGFlex** � o defindo pela Enum como: **AuthType.Basic**, atrav�s dessa defini��o, a ApiRepository utiliza o algoritmo **OCTO.Api.Client.Authenticators.BasicAuthenticator** para a conex�o �s APIs.

Os algoritmos de valida��o e autentica��o usados est�o dispon�veis no namespace **OCTO.Api.Client.Authenticators**, s�o 3:
- BasicAuthenticator
- HmacAuthenticator
- JWTcAuthenticator