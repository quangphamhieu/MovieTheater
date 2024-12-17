pipeline {
    agent any

    environment {
        DOCKER_COMPOSE_PATH = 'docker-compose.yml'
        ASPNET_IMAGE_NAME = 'movietheater-aspnet-image:latest'
    }

    stages {
        stage('Checkout Code') {
            steps {
                echo 'Checking out source code...'
                checkout scm
            }
        }

        stage('Build Docker Image') {
            steps {
                echo 'Building ASP.NET Core Docker image...'
                sh 'docker build -t ${ASPNET_IMAGE_NAME} -f Dockerfile .'
            }
        }

        stage('Deploy with Docker Compose') {
            steps {
                echo 'Deploying services using Docker Compose...'
                sh 'docker-compose -f ${DOCKER_COMPOSE_PATH} down'
                sh 'docker-compose -f ${DOCKER_COMPOSE_PATH} up -d'
            }
        }

        stage('Verify Deployment') {
            steps {
                echo 'Verifying deployment...'
                sh 'docker ps'
                script {
                    def response = sh(
                        script: "curl -s -o /dev/null -w '%{http_code}' http://localhost:5000",
                        returnStdout: true
                    ).trim()
                    if (response != '200') {
                        error "Application failed health check!"
                    } else {
                        echo "Application is up and running."
                    }
                }
            }
        }
    }

    post {
        success {
            echo 'Deployment completed successfully!'
        }
        failure {
            echo 'Deployment failed. Check logs for details.'
        }
    }
}
