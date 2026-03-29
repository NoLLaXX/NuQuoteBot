# GitHub Actions Setup

## Docker Hub Publishing

Этот workflow автоматически собирает и публикует Docker образы при создании тегов.

### Настройка GitHub Secrets

Добавьте следующие секреты в настройках репозитория (Settings → Secrets and variables → Actions):

1. **DOCKERHUB_USERNAME** - Ваш username на Docker Hub
2. **DOCKERHUB_TOKEN** - Access Token с Docker Hub (создайте в Account Settings → Security → New Access Token)

### Использование

1. Создайте и запуште тег:
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

2. GitHub Action автоматически:
   - Соберёт Docker образ
   - Опубликует образ с двумя метками:
     - `latest`
     - `v1.0.0` (имя вашего тега)
   - Удалит старые образы, оставив только последние 3

### Образы

Образы публикуются на Docker Hub: `<ваш-username>/nuquotebot`

Пример использования:
```bash
docker pull <ваш-username>/nuquotebot:latest
docker pull <ваш-username>/nuquotebot:v1.0.0
```

### Поддерживаемые платформы

- linux/amd64
- linux/arm64

### Структура образов

- **latest** - всегда указывает на последний собранный образ
- **версионные теги** - сохраняются последние 3 версии
- Старые версии автоматически удаляются
