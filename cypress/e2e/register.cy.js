describe('Rejestracja', () => {
  it('powinno zarejestrować nowego użytkownika', () => {
    cy.visit('http://localhost:7199/')

    cy.contains('Zarejestruj').click();

    cy.get('input[name="Email"]').type('przykladowymail1@wp.pl');
    cy.get('input[name="Password"]').type('Haslo123');

    cy.contains('Zarejestruj!').click();

    cy.contains('pomyślnie').should('exist');

  })
})