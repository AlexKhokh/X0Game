# X0Game
  Метод Get api/game - создание игры, возвращает uniqidentifier игры
	Метод Get api/game/<uniqidentifier> - получить контексты игры по uniqidentifier игры, не обязательный метод
	Метод Post api/game/maketurn - сделать ход, получить контекст игры, основной метод игры

	Класс GameContext - основная информация о игре:
	 GameBoard - поле игры с uniqidentifier игры(поле игры представляет собой одномерный массив с 0 по 8)
	 с тремя возможными значениями(null, true, false) где null- незаполненное поле, true- крестик, false - нолик
	 Status - состояние игры(не началась, в процессе, закончена)
	 Winner - победитель
	 Descr - описание
