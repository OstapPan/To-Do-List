import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // Дістаємо токен з локального сховища (або з твого Auth сервісу)
  // Увага: переконайся, що ключ 'jwt_token' співпадає з тим, як ти його зберігаєш при логіні!
  const token = localStorage.getItem('jwt_token'); 

  // Якщо токен є, клонуємо запит і додаємо заголовок Authorization
  if (token) {
    const clonedRequest = req.clone({
      withCredentials: true
    });
    // Відправляємо модифікований запит далі
    return next(clonedRequest);
  }

  // Якщо токена немає, просто пропускаємо запит як є
  return next(req);
};