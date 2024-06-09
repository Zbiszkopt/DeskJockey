describe('Logowanie', () => {
    it('powinno zalogować użytkownika', () => {
      cy.visit('http://localhost:7199/');
  
      cy.contains('Zaloguj').click();
  
      cy.get('input[name="Email"]').type('przykladowymail@wp.pl');
      cy.get('input[name="Password"]').type('Haslo123');
  
      cy.contains('Zaloguj!').click();

      cy.contains('Wyloguj').should('exist');
        

    });
  });