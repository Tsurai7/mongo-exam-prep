docker:
	docker-compose -f docker-compose.yml up --build -d --quiet-pull
	
docker-down:
	docker-compose -f docker-compose.yml down