# passman-back

Настройки

### **key.xml**

RSA key from xml

```xml
<RSAKeyValue>
  <Modulus> </Modulus>
  <Exponent> </Exponent>
  <P> </P>
  <Q> </Q>
  <DP> </DP>
  <DQ> </DQ>
  <InverseQ> </InverseQ>
  <D> </D>
</RSAKeyValue>
```

### **appsettings.Development.json**

```json
{
  "ConnectionStrings": {
    "Pg": "Host=localhost;Port=5443;Database=passman;Username=postgres;Password=postgres"
  },
  "MailSettings": {
    "Host": "smtp.domain.ru",
    "Port": 465,
    "Email": "username@domain.ru",
    "Password": "application-password"
  },
  "PassmanSettings": {
    "MaxLengthOfTextFields": 50,
    "MinPasswordLengthForUsers": 6,
    "FrontEndUrl": "http://localhost:3000"
  },
  "AllowedOrigins": "http://localhost:3000;http://localhost:3001"
}
```

### Environment vairables:

При запуске в контейнере рекомендуется преимущественно использовать переменные окружения для установки параметров.
Если один и тот же параметр указан в appsettings и в переменных окружения, то приоритет отдаётся в пользу переменных окружения.

```env
DB_HOST
DB_PORT
DB_NAME
DB_USER
DB_PASSWORD

EMAIL_HOST
EMAIL_PORT
EMAIL_EMAIL
EMAIL_PASSWORD

FRONTEND_URL
LOCAL_IP
```
