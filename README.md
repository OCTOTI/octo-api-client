# OCTO Api Client

O **OCTO.Api.Client** é uma biblioteca que foi criada para servir como base no consumo das APIs de Integração do MasterSGi e SGFlex.

Dentro do projeto, você encontrará a Classe **OCTO.Api.Client.ApiRepository** que é a implementação no modelo Repository para acessar as APIs de Integração.

No construtor da **ApiRepository** você deve informar o **AuthType** que é definido pela Enum: **OCTO.Api.Client.AuthType**.

O Método de Autenticação utilizado atualmente tanto pelo **MasterSGi** como pelo **SGFlex** é o defindo pela Enum como: **AuthType.Basic**, através dessa definição, a ApiRepository utiliza o algoritmo **OCTO.Api.Client.Authenticators.BasicAuthenticator** para a conexão às APIs.

Os algoritmos de validação e autenticação usados estão disponíveis no namespace **OCTO.Api.Client.Authenticators**, são 3:
- BasicAuthenticator
- HmacAuthenticator
- JWTcAuthenticator